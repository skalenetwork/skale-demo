using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Unity.InteractiveTutorials
{
    class VideoPlaybackManager
    {
        struct CacheEntry
        {
            public VideoPlayer videoPlayer;
            public RenderTexture renderTexture;
        }

        GameObject m_GameObject;
        Dictionary<VideoClip, CacheEntry> m_Cache = new Dictionary<VideoClip, CacheEntry>();

        public void OnEnable()
        {
            m_GameObject = new GameObject { hideFlags = HideFlags.HideAndDontSave };
        }

        public void OnDisable()
        {
            ClearCache();
            Object.DestroyImmediate(m_GameObject);
        }

        public Texture GetTextureForVideoClip(VideoClip videoClip)
        {
            CacheEntry cacheEntry;
            if (!m_Cache.TryGetValue(videoClip, out cacheEntry))
            {
                var renderTexture = new RenderTexture((int)videoClip.width, (int)videoClip.height, 32);
                renderTexture.hideFlags = HideFlags.HideAndDontSave;
                renderTexture.Create();

                var videoPlayer = m_GameObject.AddComponent<VideoPlayer>();
                videoPlayer.clip = videoClip;
                videoPlayer.isLooping = true;
                videoPlayer.renderMode = VideoRenderMode.RenderTexture;
                videoPlayer.targetTexture = renderTexture;
                videoPlayer.Play();

                cacheEntry.renderTexture = renderTexture;
                cacheEntry.videoPlayer = videoPlayer;
                m_Cache.Add(videoClip, cacheEntry);
            }

            return cacheEntry.renderTexture;
        }

        public void ClearCache()
        {
            foreach (var cacheEntry in m_Cache.Values)
            {
                Object.DestroyImmediate(cacheEntry.videoPlayer);
                Object.DestroyImmediate(cacheEntry.renderTexture);
            }

            m_Cache.Clear();
        }
    }
}
