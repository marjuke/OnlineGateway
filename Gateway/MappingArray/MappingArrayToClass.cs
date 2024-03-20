using Domain.RequestC;

namespace Gateway.MappingArray
{
    public class MappingArrayToClass
    {
        public static void MapArrayToClass(Model.bKashRequest.Parameter[] array, ReqCheckInformation obj)
        {
            foreach (var kvp in array)
            {
                // Use reflection to set the property based on the key
                var property = typeof(ReqCheckInformation).GetProperty(kvp.Key);

                if (property != null)
                {
                    // Convert the value to the appropriate type if needed
                    var value = Convert.ChangeType(kvp.Value, property.PropertyType);

                    // Set the property value
                    property.SetValue(obj, value);
                }
            }
        }
        public static void MapArrayToClass(Model.bKashRequest.Parameter[] array, ReqValidationPayment obj)
        {
            foreach (var kvp in array)
            {
                // Use reflection to set the property based on the key
                var property = typeof(ReqValidationPayment).GetProperty(kvp.Key);

                if (property != null)
                {
                    // Convert the value to the appropriate type if needed
                    var value = Convert.ChangeType(kvp.Value, property.PropertyType);

                    // Set the property value
                    property.SetValue(obj, value);
                }
            }
        }
        public static void MapArrayToClass(Model.bKashRequest.Parameter[] array, ReqPaymentConfirmation obj)
        {
            foreach (var kvp in array)
            {
                // Use reflection to set the property based on the key
                var property = typeof(ReqPaymentConfirmation).GetProperty(kvp.Key);

                if (property != null)
                {
                    // Convert the value to the appropriate type if needed
                    var value = Convert.ChangeType(kvp.Value, property.PropertyType);

                    // Set the property value
                    property.SetValue(obj, value);
                }
            }
        }
    }
}
