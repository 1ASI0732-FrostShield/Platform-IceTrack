namespace IceTrackPlatform.API.Feedback.Domain.Model.ValueObjects;

public record ReviewRating(int Comunicacion, int Eficiencia, int Profesionalidad)
{
    public ReviewRating() : this(0, 0, 0) { }

    public double Average => (Comunicacion + Eficiencia + Profesionalidad) / 3.0;
}

