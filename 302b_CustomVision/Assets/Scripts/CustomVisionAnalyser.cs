using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class CustomVisionAnalyser : MonoBehaviour {

    /// <summary>
    /// Unique instance of this class
    /// </summary>
    public static CustomVisionAnalyser Instance;

    /// <summary>
    /// Insert your Prediction Key here
    /// </summary>
    private string predictionKey = "bd4cfa1df00746059263547f96dcab41";

    /// <summary>
    /// Insert your prediction endpoint here
    /// </summary>
    private string predictionEndpoint = "https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Prediction/06145e3f-1522-4a1f-b499-6dc85522b45b/image?iterationId=38ab129c-d676-4582-9112-8f73651f5eb8";

    /// <summary>
    /// Byte array of the image to submit for analysis
    /// </summary>
    [HideInInspector] public byte[] imageBytes;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Initialises this class
    /// </summary>
    private void Awake()
    {
        // Allows this instance to behave like a singleton
        Instance = this;
    }


    /// <summary>
    /// Call the Computer Vision Service to submit the image.
    /// </summary>
    public IEnumerator AnalyseLastImageCaptured(string imagePath)
    {
        WWWForm webForm = new WWWForm();
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(predictionEndpoint, webForm))
        {
            // Gets a byte array out of the saved image
            imageBytes = GetImageAsByteArray(imagePath);

            unityWebRequest.SetRequestHeader("Content-Type", "application/octet-stream");
            unityWebRequest.SetRequestHeader("Prediction-Key", predictionKey);

            // The upload handler will help uploading the byte array with the request
            unityWebRequest.uploadHandler = new UploadHandlerRaw(imageBytes);
            unityWebRequest.uploadHandler.contentType = "application/octet-stream";

            // The download handler will help receiving the analysis from Azure
            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();

            // Send the request
            yield return unityWebRequest.SendWebRequest();

            string jsonResponse = unityWebRequest.downloadHandler.text;

            // The response will be in JSON format, therefore it needs to be deserialized    

            // The following lines refers to a class that you will build in later Chapters
            // Wait until then to uncomment these lines

            AnalysisObject analysisObject = new AnalysisObject();
            analysisObject = JsonConvert.DeserializeObject<AnalysisObject>(jsonResponse);
            SceneOrganiser.Instance.SetTagsToLastLabel(analysisObject);
        }
    }

    /// <summary>
    /// Returns the contents of the specified image file as a byte array.
    /// </summary>
    static byte[] GetImageAsByteArray(string imageFilePath)
    {
        FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);

        BinaryReader binaryReader = new BinaryReader(fileStream);

        return binaryReader.ReadBytes((int)fileStream.Length);
    }
}
