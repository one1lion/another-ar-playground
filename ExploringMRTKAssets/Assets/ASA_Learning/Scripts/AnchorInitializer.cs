using Microsoft.Azure.SpatialAnchors.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Text.RegularExpressions;
using UnityEngine;

[JsonConverter(typeof(StringEnumConverter))]
public enum FileConfigType {
  Json = 1,
  Ini,
  Xml
}

[CreateAssetMenu(fileName = "AnchorInitializer", menuName = "Azure Spatial Anchors/Initializer")]
public class AnchorInitializer : ScriptableObject {
  [SerializeField]
  protected TextAsset configurationFile;
  public TextAsset ConfigurationFile => configurationFile;

  private SpatialAnchorConfig spatialAnchorConfig;
  public SpatialAnchorConfig SpatialAnchorConfig => spatialAnchorConfig;

  private void Awake() {
    Initialize();
  }

  public void Initialize() {
    if (configurationFile is null) {
      // Ignore the use of this and let it default to the configuration file from the inspector for this GameObject
      Debug.LogWarning("Specifying the Azure Spatial Anchors Configuration is not recommended using the settings in the Inspector.  Instead, provide a TextAsset file containing the serialized or parsable configuration.  The file can be an .json, .ini, or .xml formatted file with this extension.  A sample of each type can be found in Assets/Resources/Asa/");
      return;
    }

    var whiteSpace = new Regex(@"[\r\n\t\s]");
    var noWhiteSpaceText = whiteSpace.Replace(configurationFile.text.ToLower(), "");
    var fileType = noWhiteSpaceText.StartsWith("{") ? FileConfigType.Json // Assume a well-formed JSON file
      : noWhiteSpaceText.StartsWith("<?xml") ? FileConfigType.Xml // Assume an XML file
        : FileConfigType.Ini; // Assume parsable text where each line is a Key/Value pair, separated by '=' character

    Debug.Log($"File type: {fileType}");
    SpatialAnchorConfig sac = default;

    spatialAnchorConfig = fileType switch {
      FileConfigType.Json => ReadJsonSettings(configurationFile.text),
      FileConfigType.Ini => ReadIniSettings(configurationFile.text),
      FileConfigType.Xml => ReadXmlSettings(configurationFile.text),
      _ => throw new TypeLoadException("The provided configuration file was not in an expected format. Provide a TextAsset file containing the serialized or parsable configuration with an appropriate extension (.json, .ini, or .xml) with properly formatted content.  A sample of each type can be found in Assets/Resources/Asa/")
    };

    Debug.Log(JsonConvert.SerializeObject(sac, new JsonSerializerSettings() {
      ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    }));

    if (sac is null) {
      Debug.LogWarning("The provided configuration file could not be read.  Provide a TextAsset file containing the serialized or parsable configuration with an appropriate extension (.json, .ini, or .xml) with properly formatted content.  A sample of each type can be found in Assets/Resources/Asa/");
      return;
    }
  }

  public void ApplySpatialAnchorSettings(SpatialAnchorManager sam) {
    sam.SpatialAnchorsAccountDomain = spatialAnchorConfig.SpatialAnchorsAccountDomain;
    switch (spatialAnchorConfig.AuthenticationMode) {
      case AuthenticationMode.ApiKey:
        sam.SpatialAnchorsAccountId = spatialAnchorConfig.SpatialAnchorsAccountId;
        sam.SpatialAnchorsAccountKey = spatialAnchorConfig.SpatialAnchorsAccountKey;
        break;
      case AuthenticationMode.AAD:
        sam.ClientId = spatialAnchorConfig.ClientId;
        sam.TenantId = spatialAnchorConfig.TenantId;
        break;
    }
  }

  private SpatialAnchorConfig ReadIniSettings(string text) {
    throw new NotImplementedException();
  }

  private SpatialAnchorConfig ReadXmlSettings(string text) {
    throw new NotImplementedException();
  }

  private SpatialAnchorConfig ReadJsonSettings(string fileText) {
    return JsonConvert.DeserializeObject<SpatialAnchorConfig>(fileText, new JsonSerializerSettings() {
      ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
      NullValueHandling = NullValueHandling.Ignore
    });
  }
}
