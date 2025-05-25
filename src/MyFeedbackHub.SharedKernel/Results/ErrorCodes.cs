namespace MyFeedbackHub.SharedKernel.Results;

public sealed class ErrorCodes
{
    public sealed class Auth
    {
        public const string UsernameOrPasswordInvalid = "username_password_invalid";
    }

    public sealed class User
    {
        public const string PasswordInvalid = "password_invalid";
        public const string UsernameInvalid = "username_invalid";
        public const string UserInvalid = "user_invalid";
        public const string InvitationTokenInvalid = "invitation_token_invalid";
        public const string NotFound = "not_found";
        public const string PasswordNotStrong = "password_not_strong";
    }

    public sealed class Project
    {
        public const string ProjectNameInvalid = "project_name_invalid";
        public const string ProjectInvalid = "project_invalid";
        public const string NotFound = "not_found";
    }

    public sealed class Organization
    {
        public const string OrganizationInvalid = "organization_invalid";
        public const string OrganizationNameInvalid = "organization_name_invalid";
    }

    public sealed class Feedback
    {
        public const string FeedbackInvalid = "feedback_invalid";
        public const string TitleInvalid = "feedback_title_invalid";
        public const string DescriptionInvalid = "feedback_description_invalid";
        public const string NotFound = "not_found";
    }

    public sealed class Comment
    {
        public const string NotFound = "not_found";
        public const string CommentInvalid = "comment_invalid";
    }
}
