using System.Reflection;
using Flunt.Notifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PlaylistRecommender.Api.JsonContract
{
    public class FluntJsonResolver : DefaultContractResolver
    {
        public string[] Members { get; set; }
        public FluntJsonResolver(params string[] _members)
        {
            Members = _members;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            property.NullValueHandling = NullValueHandling.Ignore;

            if(property.PropertyName == nameof(Notifiable.Valid) ||
               property.PropertyName == nameof(Notifiable.Invalid))
            {
                return null;
            }

            if(property.PropertyName == nameof(Notifiable.Notifications))
            {     
                property.ShouldSerialize = o => { 
                    return ((Notifiable)o).Notifications != null && ((Notifiable)o).Notifications.Count > 0; 
                };
            }

            return property;
        }
    }
}