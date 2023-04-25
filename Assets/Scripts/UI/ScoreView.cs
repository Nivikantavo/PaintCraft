using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private TMP_Text _rank;
    [SerializeField] private TMP_Text _score;
    [SerializeField] private TMP_Text _nickname;

    public void Initialize(int rank, int score, string nickname)
    {
        _rank.text = rank.ToString();
        _score.text = score.ToString();

        if (string.IsNullOrEmpty(nickname))
            nickname = "Anonymous";

        _nickname.text = nickname;
    }
}
