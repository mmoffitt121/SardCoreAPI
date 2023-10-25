using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Runtime.InteropServices.ObjectiveC;

namespace SardCoreAPI.Utility.Error
{
    public static class MapResultExtentions
    {
        /// <summary>
        /// Handles MySqlException errors
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static IActionResult Handle(this MySqlException ex)
        {
            switch (ex.Number)
            {
                case 1062:
                    return new ConflictObjectResult("An item in your request already exists or is a duplicate.");
                case 1451:
                    return new ConflictObjectResult("This item is in use by another item, and cannot be deleted.");
                default:
                    return new BadRequestObjectResult("An unknown error occured. Check the server logs for more information.");
            }
        }

        /// <summary>
        /// Handles MySqlException errors
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static IActionResult Handle(this IOException ex)
        {
            if (ex.GetType() == typeof(System.IO.DirectoryNotFoundException))
            {
                return new NotFoundObjectResult("Directory not found.");
            }
            return new BadRequestObjectResult("An unknown error occured. Check the server logs for more information.");
        }

        /// <summary>
        /// Handles generic exception errors
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static IActionResult Handle(this Exception ex)
        {
            if (ex is MySqlException)
            {
                return ((MySqlException)ex).Handle();
            }
            if (ex is IOException)
            {
                return ((IOException)ex).Handle();
            }
            if (ex is IOException)
            {
                return ((IOException)ex).Handle();
            }
            return new BadRequestObjectResult("An unknown error occured. Check the server logs for more information.");
        }

        public static IActionResult HandlePost(this int result)
        {
            if (result > 0) return new OkResult();

            if (result == 0) return new NotFoundResult();

            return new BadRequestResult();
        }

        public static IActionResult HandlePut(this int result)
        {
            if (result > 0) return new OkResult();

            if (result == 0) return new NotFoundResult();

            return new BadRequestResult();
        }

        public static IActionResult HandleDelete(this int result)
        {
            if (result > 0) return new OkResult();

            if (result == 0) return new NotFoundResult();

            return new BadRequestResult();
        }
    }
}
