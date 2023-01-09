namespace EasyCI.Domain.Logic.Helpers
{
    public static class ObjectExtensions
    {
        public static T AssertValue<T>(this T? value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Value cannot be null");
            }

            return value;
        }
    }
}
