namespace TrackerDeFavorisApi.Models
{
    public class OmdbSearchResponse
    {
        public string Response { get; set; } = "";
        public List<OmdbFilm>? Search { get; set; }
    }

    public class OmdbFilm
    {
        public string Title { get; set; } = "";
        public string Year { get; set; } = "";
        public string ImdbID { get; set; } = "";
        public string Poster { get; set; } = "";
    }
}