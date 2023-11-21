using System.ComponentModel.DataAnnotations;

namespace Films.Annotations
{
    // Перевіряє жанр на коректність
    public class MyGenresAttribute: ValidationAttribute
    {
        private static string[] myGenres;
        public MyGenresAttribute(string[] Genres)
        {
            myGenres = Genres;
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                string strval = value.ToString();
                if (myGenres.Any(genre => genre == strval)) return true;
            }
            return false;
        }
    }
}
