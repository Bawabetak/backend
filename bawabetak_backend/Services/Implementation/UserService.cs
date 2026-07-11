namespace bawabetak_backend.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ISendVerificationFactory _sendVerificationFactory;
        private readonly ICheckVerificationFactory _checkVerificationFactory;
        private readonly ILoginFactory _loginFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICacheHelper _cacheHelper;
        private readonly IRefreshTokenFactory _refreshTokenFactory;
        private readonly IFileService _fileService;

        public UserService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            ISendVerificationFactory sendVerificationFactory,
            ICheckVerificationFactory checkVerificationFactory,
            ILoginFactory loginFactory,
            IHttpContextAccessor httpContextAccessor,
            ICacheHelper cacheHelper,
            IRefreshTokenFactory refreshTokenFactory,
            IFileService fileService
            )
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _sendVerificationFactory = sendVerificationFactory;
            _checkVerificationFactory = checkVerificationFactory;
            _loginFactory = loginFactory;
            _httpContextAccessor = httpContextAccessor;
            _cacheHelper = cacheHelper;
            _refreshTokenFactory = refreshTokenFactory;
            _fileService = fileService;
        }

        public async Task SendVerificationCodeAsync(SendVerificationDto dto)
        {
            var strategy = _sendVerificationFactory.GetStrategy(dto.Type);
            await strategy.SendVerficationCode(dto.Email);
        }

        public async Task VerifyCodeAsync(VerifyCodeDto dto)
        {
            var strategy = _checkVerificationFactory.GetStrategy(dto.Type);
            await strategy.VerifyCodeAsync(dto.Email, dto.Code);
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto dto)
        {
            var isApproved = await _cacheHelper.GetAsync<bool>($"RegisterApproved_{dto.Email}");
            if (!isApproved)
            {
                throw new BadRequestCustomException(ResponseKeys.EmailNotVerified);
            }

            var existingUser = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                throw new BadRequestCustomException(ResponseKeys.EmailAlreadyExists);
            }
            var photoFileName = await _fileService.SaveFileAsync(dto.Photo, FileCategory.UserPhoto);
            var identityFileName = await _fileService.SaveFileAsync(dto.IdentityDocument, FileCategory.UserIdentityDocument);

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                Address = dto.Address,
                NationalNumber = dto.NationalNumber,
                Photo = photoFileName,
                IdentityDocument = identityFileName

            };

            var result = await _userRepository.CreateUserAsync(user, dto.Password);
          

            var roleResult = await _roleRepository.AddUserToRoleAsync(user, "User");
            if (!roleResult.Succeeded)
            {
                await _userRepository.DeleteUserAsync(user); 
                throw new BadRequestCustomException(ResponseKeys.RoleAssignmentFailed);
            }

            await _cacheHelper.RemoveAsync($"RegisterApproved_{dto.Email}");
            return result;
        }

        public async Task<AuthTokenResponseDto> LoginAsync(LoginDto dto)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            string clientType = "Web"; 

            if (httpContext != null && httpContext.Request.Headers.TryGetValue("X-Client-Type", out var headerValue))
            {
                clientType = headerValue.ToString();
            }

            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null) throw new NotFoundCustomException(ResponseKeys.InvalidEmailOrPassword);

            var isPasswordValid = await _userRepository.IsSamePasswordAsync(user, dto.Password);
            if (!isPasswordValid) throw new NotFoundCustomException(ResponseKeys.InvalidEmailOrPassword);

            var roles = await _roleRepository.GetRolesByUserAsync(user);

            var strategy = _loginFactory.GetStrategy(clientType);
            return await strategy.Login(user, roles);
        }

        public async Task ForgetPasswordAsync(ForgetPasswordDto dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null)
            {
                throw new NotFoundCustomException(ResponseKeys.UserNotFound);
            }
            var approvalKey = $"ResetApproved_{dto.Email}";
            var IsApproved = await _cacheHelper.GetAsync<bool>(approvalKey);
            if (!IsApproved)
            {
                throw new BadRequestCustomException(ResponseKeys.ResetNotApproved);
            }
            await _userRepository.ResetPasswordAsync(user, dto.Password);
            await _cacheHelper.RemoveAsync(approvalKey);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ChangePasswordDto dto)
        {
            var IsSamePassword = dto.NewPassword == dto.OldPassword;
            if (IsSamePassword)
            {
                throw new BadRequestCustomException(ResponseKeys.NewPasswordCannotBeSameAsOld);
            }

            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null) throw new NotFoundCustomException(ResponseKeys.UserNotFound);

            var isOldPasswordValid = await _userRepository.IsSamePasswordAsync(user, dto.OldPassword);
            if (!isOldPasswordValid)
            {
                throw new BadRequestCustomException(ResponseKeys.InvalidOldPassword);
            }
          

            var result = await _userRepository.ChangePassword(user, dto.OldPassword, dto.NewPassword);
            return result;
        }
        public async Task<AuthTokenResponseDto> RefreshTokenAsync(RefreshTokenDto dto)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            string clientType = "Web";

            if (httpContext != null && httpContext.Request.Headers.TryGetValue("X-Client-Type", out var headerValue))
            {
                clientType = headerValue.ToString();
            }

            var strategy = _refreshTokenFactory.GetStrategy(clientType);
            return await strategy.RefreshAsync(dto?.RefreshToken);
        }
    }
}