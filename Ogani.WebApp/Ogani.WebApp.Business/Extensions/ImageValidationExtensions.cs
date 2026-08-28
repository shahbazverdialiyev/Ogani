using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Extensions
{

    public static class ImageValidationExtensions
    {
        private static readonly string[] AllowedExtensions =
        {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
        };

        private const int MaxImageSizeInMB = 2;
        private const long MaxImageSize =
            MaxImageSizeInMB * 1024 * 1024;

        public static IRuleBuilderOptions<T, IFormFile?> ValidateImage<T>(
            this IRuleBuilder<T, IFormFile?> ruleBuilder)
        {
            return ruleBuilder
                .Must(file => file is null || AllowedExtensions.Contains(Path.GetExtension(file.FileName).ToLowerInvariant()))
                .WithMessage($"Only the following file types are allowed: " + $"{string.Join(", ", AllowedExtensions)}.")

                .Must(file => file is null || file.Length <= MaxImageSize)
                .WithMessage($"Image size must be less than {MaxImageSizeInMB} MB.");
        }
    }
}
