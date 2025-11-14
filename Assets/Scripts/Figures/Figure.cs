using Utils;

public class Figure
{
    public FigureType Type;
    public readonly Team Team;

    public Figure(FigureType type, Team team)
    {
        Type = type;
        Team = team;
    }
}