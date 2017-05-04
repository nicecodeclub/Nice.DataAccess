using Grit.Net.DataAccess.Attributes;
using Grit.Net.DataAccess.Models;

namespace Tingche.Easy.Park.Support.Web.Model
{
    [Table(Name = "dict_city")]
    public class City : Entity
    {
        [Column(Name = "N_CITYID")]
        []
        public int CityID { get; set; }
        [Column(Name = "S_CITYNAME")]
        public string CityName { get; set; }
        [Column(Name = "N_PROVID")]
        public int ProvId { get; set; }
        [Column(Name = "S_STATE")]
        public string State { get; set; }
    }
}