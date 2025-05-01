namespace MyFeedbackHub.SharedKernel.Results;

public sealed class ErrorCodes
{
    public sealed class User
    {
        public const string PasswordInvalid = "password_invalid";
        public const string UsernameInvalid = "username_invalid";
        public const string UserInvalid = "user_invalid";
    }

    public sealed class Project
    {
        public const string ProjectNameInvalid = "project_name_invalid";
        public const string ProjectInvalid = "project_invalid";
    }

    public sealed class Organization
    {
        public const string OrganizationInvalid = "organization_invalid";
    }
}
