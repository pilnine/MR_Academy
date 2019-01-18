using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoController : MonoBehaviour {

    /// <summary> 
    /// Provides Singleton-like behaviour to this class. 
    /// </summary> 
    public static VideoController instance;

    /// <summary> 
    /// Reference to the Camera VideoPlayer Component.
    /// </summary> 
    private VideoPlayer videoPlayer;

    /// <summary>
    /// Reference to the Camera AudioSource Component.
    /// </summary> 
    private AudioSource audioSource;

    /// <summary> 
    /// Reference to the texture used to project the video streaming 
    /// </summary> 
    private RenderTexture videoStreamRenderTexture;

    /// <summary>
    /// Insert here the first video endpoint
    /// </summary>
    private string video1endpoint = "https://mracademystroage.blob.core.windows.net/asset-21c166d2-fd4b-4bd6-9778-d91cba4c635e/Ayutthaya - Easy Tripod Paint _ 360_VR Master Series _ Free Download.mp4?sv=2017-04-17&sr=c&si=8679f5d0-da0f-4c61-bebc-6665fe61dc8f&sig=HUbkW1N0nDYazUtN0JjK5%2BA2ccMRSKuvxhlasitYiQw%3D&st=2019-01-18T23%3A18%3A00Z&se=2119-01-18T23%3A18%3A00Z";

    /// <summary>
    /// Insert here the second video endpoint
    /// </summary>
    private string video2endpoint = "http://mracademystroage.blob.core.windows.net/asset-af47d3be-7577-4f99-9d8f-2e17e089e49d/Ayutthaya - needs stabilization _1920x960_AACAudio_4440.mp4?sv=2017-04-17&sr=c&si=e024a1d1-b275-4433-9b5e-304ff13ced05&sig=tvJ4Jig2hfC7r11DGIJ17lTzFL0RfGxytlc%2B4Dc93zQ%3D&st=2019-01-18T23%3A28%3A42Z&se=2119-01-18T23%3A28%3A42Z";

    /// <summary> 
    /// Reference to the Inside-Out Sphere. 
    /// </summary> 
    public GameObject sphere;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        Application.runInBackground = true;
        StartCoroutine(PlayVideo());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private IEnumerator PlayVideo()
    {
        // create a new render texture to display the video 
        videoStreamRenderTexture = new RenderTexture(2160, 1440, 32, RenderTextureFormat.ARGB32);

        videoStreamRenderTexture.Create();

        // assign the render texture to the object material 
        Material sphereMaterial = sphere.GetComponent<Renderer>().sharedMaterial;

        //create a VideoPlayer component 
        videoPlayer = gameObject.AddComponent<VideoPlayer>();

        // Set the video to loop. 
        videoPlayer.isLooping = true;

        // Set the VideoPlayer component to play the video from the texture 
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;

        videoPlayer.targetTexture = videoStreamRenderTexture;

        // Add AudioSource 
        audioSource = gameObject.AddComponent<AudioSource>();

        // Pause Audio play on Awake 
        audioSource.playOnAwake = true;
        audioSource.Pause();

        // Set Audio Output to AudioSource 
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.source = VideoSource.Url;

        // Assign the Audio from Video to AudioSource to be played 
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        // Assign the video Url depending on the current scene 
        switch (SceneManager.GetActiveScene().name)
        {
            case "VideoScene1":
                videoPlayer.url = video1endpoint;
                break;

            case "VideoScene2":
                videoPlayer.url = video2endpoint;
                break;

            default:
                break;
        }

        //Set video To Play then prepare Audio to prevent Buffering 
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        sphereMaterial.mainTexture = videoStreamRenderTexture;

        //Play Video 
        videoPlayer.Play();

        //Play Sound 
        audioSource.Play();

        while (videoPlayer.isPlaying)
        {
            yield return null;
        }
    }

    public void ChangeScene()

    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name == "VideoScene1" ? "VideoScene2" : "VideoScene1");
    }
}
