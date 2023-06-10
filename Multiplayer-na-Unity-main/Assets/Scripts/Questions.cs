[System.Serializable]
public class Questions 
{
    public string Question;
    public string[] Awnsers;
    public string CorrectAwnser;

    public string GetQuestion { get { return Question; } }
    public string[] GetAwnsers { get { return Awnsers; } }
    public string GetCorrectAwnser { get { return CorrectAwnser; } }
    
    public Questions(string Question, string[] Awnsers, string CorrectAwnser)
    {
        this.Question = Question;
        this.Awnsers = Awnsers;
        this.CorrectAwnser = CorrectAwnser;
    }
}
