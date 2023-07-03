﻿
namespace BallastLane.Presentation.Contracts;

public sealed record RegisterUserRequest(
    string Email,
    string FirstName,
    string LastName,
    string Password);
