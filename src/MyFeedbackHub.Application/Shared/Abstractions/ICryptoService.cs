﻿namespace MyFeedbackHub.Application.Shared.Abstractions;

public interface ICryptoService
{
    string HashPassword(string password, out byte[] salt);

    bool VerifyPassword(string password, string passwordHashBase64, string salt);
}