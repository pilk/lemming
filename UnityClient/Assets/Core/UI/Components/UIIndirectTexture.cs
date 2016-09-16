using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(RawImage))]
public class UIIndirectTexture : MonoBehaviour
{
    public enum UnloadType
    {
        OnDisable,
        OnDestroy,
    }

    public IndirectTexture m_startTexture = new IndirectTexture();
    public RawImage m_target = null;
    public UnloadType m_unloadType = UnloadType.OnDisable;

    private IndirectTexture m_textureReference = null;
    private bool m_dynamic = false;

    public IndirectTexture textureReference
    {
        get { return m_textureReference; }
        set
        {
            if (m_textureReference == null || m_textureReference.assetName != value.assetName)
            {
                m_dynamic = true;

                // Unload previous texture
                Unload();
                m_textureReference = value;
                Load();
            }
        }
    }

    private bool IsValidTextureReference(IndirectTexture textureReference)
    {
        if (textureReference == null) return false;
        if (textureReference.Empty) return false;
        return true;
    }

    private void Reset()
    {
        m_target = GetComponent<RawImage>();
#if UNITY_EDITOR
        if (m_target != null && m_target.texture != null)
        {
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(m_target.texture);
            string assetGuid = UnityEditor.AssetDatabase.AssetPathToGUID(assetPath);
            if (assetPath.Contains("Assets/Resources") == false)
            {
                Debug.LogWarning("Current detected texture asset is not under Resources. Was this intended?");
                return;
            }

            assetPath = assetPath.Replace("Assets/Resources/", "");
            int fileExtPos = assetPath.LastIndexOf(".");
            if (fileExtPos >= 0)
                assetPath = assetPath.Substring(0, fileExtPos);

            this.m_startTexture = new IndirectTexture(assetPath);
            this.m_startTexture.assetGuid = assetGuid;
            if (this.isActiveAndEnabled == false)
            {
                m_target.texture = null;
            }
        }
#endif
    }

    private void Awake()
    {
        m_target = GetComponent<RawImage>();
    }

    private void OnEnable()
    {
        if (m_dynamic == false && IsValidTextureReference(textureReference) == false && IsValidTextureReference(m_startTexture))
        {
            // Unload previous texture
            Unload();
            m_textureReference = m_startTexture;
            Load();
        }
        else if (m_unloadType == UnloadType.OnDisable)
        {
            Load();
        }
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        if (Application.isPlaying == false)
        {
            // No need to unload and remove the texture reference if we're currently editing it in the editor
            return;
        }
#endif
        if (m_unloadType == UnloadType.OnDisable)
        {
            Unload();
        }
    }

    private void OnDestroy()
    {
#if UNITY_EDITOR
        if (Application.isPlaying == false)
        {
            // No need to unload and remove the texture reference if we're currently editing it in the editor
            return;
        }
#endif
        if (m_unloadType == UnloadType.OnDestroy)
        {
            Unload();
        }
    }

    public void Load()
    {
        if (this.isActiveAndEnabled == false)
            return;

        //Debug.Log(string.Format("Load for [{0}] {1}", this.gameObject.name, IsTextureReferencedByMe()));
        if (IsValidTextureReference(m_textureReference) && IsTextureReferencedByMe() == false)
        {
            m_textureReference.Load();
            m_target.texture = m_textureReference.LoadedResource;
        }
    }

    public void Unload()
    {
        //Debug.Log(string.Format("Unload for [{0}] {1}", this.gameObject.name, IsTextureReferencedByMe()));
        if (IsValidTextureReference(m_textureReference) && IsTextureReferencedByMe())
        {
            m_textureReference.Unload();
        }
        m_target.texture = null;
        //m_textureReference = null;
    }

    private bool IsTextureReferencedByMe()
    {
        if (IsValidTextureReference(m_textureReference) == false)
            return false;

        if (m_textureReference.Loaded == false)
            return false;

        if (m_target.texture == null)
            return false;

        if (m_textureReference.LoadedResource == null)
            return false;


        return m_target.texture.name.Equals(m_textureReference.assetName);
    }
}
