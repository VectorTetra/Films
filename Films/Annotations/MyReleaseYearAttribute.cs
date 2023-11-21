using System.ComponentModel.DataAnnotations;

namespace Films.Annotations
{
    // Перевіряє рік випуску на коректність
    public class MyReleaseYearAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                int intval = Convert.ToInt32(value);
                if (intval >= 1895 && intval <= DateTime.Now.Year ) return true;
            }
            return false;
        }
    }
}
