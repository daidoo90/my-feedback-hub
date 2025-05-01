namespace MyFeedbackHub.Api.Features.Feedback.GenerateQRCode;

//public sealed class GenerateQRCodeEndpoint : ICarterModule
//{
//    public void AddRoutes(IEndpointRouteBuilder app)
//    {
//        app.MapPost("/feedbacks/qr-code", () =>
//        {
//            using var qrGenerator = new QRCodeGenerator();
//            string feedbackPageUrl = "http://94.158.24.220:5263/Feedback";
//            using var qrCodeData = qrGenerator.CreateQrCode(feedbackPageUrl, QRCodeGenerator.ECCLevel.Q);
//            using var qrCode = new PngByteQRCode(qrCodeData);
//            var bytes = qrCode.GetGraphic(20);

//            return Results.File(bytes, "image/png");
//        })
//        .WithName("GR Code")
//        .Produces(StatusCodes.Status200OK)
//        .WithSummary("Generate QR Code")
//        .WithDescription("Generate QR Code")
//        .WithTags("Feedback")
//        .RequireAuthorization();
//    }
//}
