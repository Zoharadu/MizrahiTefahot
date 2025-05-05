﻿namespace Common
{
    // Represents login credentials for authenticatio
    public class LoginRequest
    {
        // The username of the user attempting to log in
        public string Username { get; set; }

        // The password of the user attempting to log in
        public string Password { get; set; }
    }
}
