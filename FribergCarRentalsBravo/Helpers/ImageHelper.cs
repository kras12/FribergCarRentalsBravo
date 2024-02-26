using FribergCarRentalsBravo.DataAccess.Entities;
using Humanizer;
using Microsoft.CodeAnalysis.CSharp;

namespace FribergCarRentalsBravo.Helpers
{
    /// <summary>
    /// A helper class for saving and deleting images to and from the disk.
    /// </summary>
    public static class ImageHelper
    {
        #region Constants

        /// <summary>
        /// The url for the image folder.
        /// </summary>
        private const string ImageFolderUrl = "/images";

        /// <summary>
        /// The route path for the image folder on the local disk.
        /// </summary>
        private const string LocalDiskImageFolderRoutePart = "wwwroot/images";

        /// <summary>
        /// The max number of attempts to try and save a file to disk.
        /// </summary>
        private const int MaxDiskSaveAttemptsPerFile = 1_000;

        /// <summary>
        /// The largest number number suffix for image files.
        /// </summary>
        private const int MaxFileNumberSuffix = 10_000;

        /// <summary>
        /// The smallest number number suffix for image files.
        /// </summary>
        private const int MinFileNumberSuffix = 1_000;

        #endregion

        #region Properties

        /// <summary>
        /// The path to the image folder on the local disk.
        /// </summary>
        private static string LocalDiskImageFolderPath
        {
            get
            {
                return Path.Combine(Directory.GetCurrentDirectory(), LocalDiskImageFolderRoutePart);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes the image file from the local disk. 
        /// </summary>
        /// <param name="imageFileName">The name of the image to delete.</param>
        public static void DeleteImageFromDisk(string imageFileName)
        {
            DeleteImagesFromDisk(new List<string>() { imageFileName });
        }

        /// <summary>
        /// Deletes images files from the local disk. 
        /// </summary>
        /// <param name="imageFileNames">A collection of images to delete.</param>
        /// <exception cref="ArgumentException"></exception>
        public static void DeleteImagesFromDisk(IEnumerable<string> imageFileNames)
        {
            #region Checks

            if (!imageFileNames.Any())
            {
                throw new ArgumentException($"The collection '{nameof(imageFileNames)}' can't be empty.");
            }

            #endregion

            foreach (var imageFileName in imageFileNames)
            {
                File.Delete(Path.Combine(LocalDiskImageFolderPath, imageFileName));
            }
        }

        /// <summary>
        /// Returns the url for the image.
        /// </summary>
        /// <param name="image">The image to retrive the url for.</param>
        /// <returns>The url of the image.</returns>
        public static string GetImageFileUrl(CarImage image)
        {
            return $"{ImageFolderUrl}/{image.FileName}";
        }

        /// <summary>
        /// Saves a collection of uploaded images to the local disk. 
        /// </summary>
        /// <param name="imageFiles">A collection of uploaded images to save to the disk.</param>
        /// <returns>A collection of strings containing the file names of the images that were saved.</returns>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static async Task<List<string>> SaveUploadedImagesToDisk(IEnumerable<IFormFile> imageFiles)
        {
            #region Checks

            if (!imageFiles.Any())
            {
                throw new ArgumentException($"The collection '{nameof(imageFiles)}' can't be empty.");
            }

            #endregion

            List<string> result = new();

            try
            {
                var random = new Random();

                if (!Directory.Exists(LocalDiskImageFolderPath))
                {
                    Directory.CreateDirectory(LocalDiskImageFolderPath);
                }

                foreach (var imageFile in imageFiles)
                {
                    FileInfo fileInfo = new FileInfo(imageFile.FileName);
                    string fileName = "";
                    string filePath = "";

                    for (int i = 0; i < MaxDiskSaveAttemptsPerFile; i++)
                    {
                        fileName = $"{Path.GetFileNameWithoutExtension(fileInfo.Name)}-{random.Next(MinFileNumberSuffix, MaxFileNumberSuffix)}{fileInfo.Extension}";
                        filePath = Path.Combine(LocalDiskImageFolderPath, fileName);

                        if (!File.Exists(filePath))
                        {
                            break;
                        }
                    }

                    using (var stream = new FileStream(filePath, FileMode.CreateNew))
                    {
                        await imageFile.CopyToAsync(stream);
                        result.Add(fileName);
                    }
                }

                return result;
            }
            catch (IOException ex)
            {
                try
                {
                    DeleteImagesFromDisk(result);
                }
                catch (IOException)
                {
                    throw new IOException("Failed to cleanup all images from the disk after the image save process failed.", ex);
                }

                throw;
            }
        }

        #endregion
    }
}
