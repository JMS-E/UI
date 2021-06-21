using UnityEngine;
using UnityEngine.Video;

public class MovieController : MonoBehaviour
{
    //get a reference in the inspector for the videoplayer
    public VideoPlayer videoPlayer;

    public void ToggleMoviePausePlay() 
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
        else
        {
            videoPlayer.Play();
        }
    }

    public void ToggleMovieStartStop()
    {
        //NOT! because is the player is on pause it will not register for stop.
        if (videoPlayer.isPaused || videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }
        else
        {
            videoPlayer.Play();
        }
    }
}
