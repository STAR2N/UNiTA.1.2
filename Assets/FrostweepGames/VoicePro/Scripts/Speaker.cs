﻿using System.Collections.Generic;
using UnityEngine;

namespace FrostweepGames.VoicePro
{
    /// <summary>
    /// Basic speaker of voice chat
    /// </summary>
    public class Speaker
    {
        private GameObject _selfObject;
        public Transform transform => _selfObject.transform;

        private AudioSource _source;

        private AudioClip _workingClip;

        private Buffer _buffer;

        private bool _audioClipReadyToUse;

        private float _delay;

        private float _notActiveTime;

        private int _maxNotActiveTime = 300; // if 5 minutes client not receives any data then its inactive

        /// <summary>
        /// Id of as speaker
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// name of a speaker
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Does speaker muted or not
        /// </summary>
        public bool IsMute { get { return _source.mute; } set { _source.mute = value; } }

        /// <summary>
        /// Is client active or not. if inactive it will be destroyed and marked as inactive. look at _maxNotActiveTime rtegardign max time of inactivity
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Returns state of speaker - does it says something or not
        /// </summary>
        public bool Playing { get; private set; }

        public Speaker(int id, string name, Transform parent)
        {
            Id = id;
            Name = name;
            _selfObject = new GameObject(Name);
            _selfObject.transform.SetParent(parent);
            _source = _selfObject.AddComponent<AudioSource>();

            _buffer = new Buffer();

            InitSound();

            IsActive = true;
        }

        /// <summary>
        /// Do whole processing of playing data from network
        /// </summary>
        public void Update()
        {
            try
            {
                _audioClipReadyToUse = _buffer.data.Count > 0;//= Constants.SampleRate * Constants.RecordingTime;

                if (Playing)
                {
                    _delay -= Time.deltaTime;

                    if (_delay <= 0)
                    {
                        _source.Stop();
                        //InitSound();
                        Playing = false;
                    }
                }

                if (!Playing)
                {
                    if (_audioClipReadyToUse)
                    {
                        List<float> chunk;

                        if (_buffer.data.Count >= Constants.SampleRate)
                        {
                            chunk = _buffer.data.GetRange(0, Constants.SampleRate);
                            _buffer.data.RemoveRange(0, Constants.SampleRate);

                            _delay = 1f;// (float)chunk.Count / (float)Constants.SampleRate;
                        }
                        else
                        {
                            int bufferSize = _buffer.data.Count;

                            chunk = new List<float>();
                            chunk.AddRange(_buffer.data);
                            _buffer.data.Clear();

                            for (int i = bufferSize; i < Constants.SampleRate; i++)
                                chunk.Add(0);

                            _delay = (float)bufferSize / (float)Constants.SampleRate;
                        }

                        _workingClip.SetData(chunk.ToArray(), 0);
                        _source.Play();

                        Playing = true;
                    }

                    _notActiveTime += Time.deltaTime;
                }

                IsActive = _notActiveTime < _maxNotActiveTime;
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Spekaer exception: " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        /// <summary>
        /// Fills sampels buffer from data from network converted to float array
        /// </summary>
        /// <param name="bytes"></param>
        public void HandleRawData(byte[] bytes)
        {
            _buffer.data.AddRange(AudioConverter.ByteToFloat(bytes));
            _notActiveTime = 0f;
        }

        /// <summary>
        /// Destroys this speaker object with cleaning data
        /// </summary>
        public void Dispose()
        {
            if (_selfObject == null)
                return;

            _source.Stop(); 
            _buffer.data.Clear();
            _buffer.position = 0;

            if (_workingClip != null)
            {
                Object.Destroy(_workingClip);
            }
            Object.Destroy(_selfObject);
            _selfObject = null;
        }

        /// <summary>
        /// Initializes AudioClip and sets it to audio source
        /// </summary>
        private void InitSound()
		{
            if (_workingClip != null || _workingClip)
                MonoBehaviour.Destroy(_workingClip);

            _workingClip = AudioClip.Create("BufferedClip_" + Id, Constants.SampleRate * Constants.RecordingTime, 1, Constants.SampleRate, false);
            _source.clip = _workingClip;
        }

        /// <summary>
        /// Basic data buffer of samples
        /// </summary>
        private class Buffer
        {
            public int position;
            public List<float> data;

            [UnityEngine.Scripting.Preserve]
            public Buffer()
            {
                position = 0;
                data = new List<float>();
            }
        }
    }
}