using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Community.PowerToys.Run.Plugin.Definition
{
    internal class AudioManager : IDisposable
    {
        private MediaPlayer _mediaPlayer;
        private readonly object _lock = new object();
        private bool _disposed;

        public AudioManager()
        {
            if (Application.Current?.Dispatcher != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _mediaPlayer = new MediaPlayer();
                });
            }
            else
            {
                _mediaPlayer = new MediaPlayer();
            }
        }

        public bool PlayAudio(string url)
        {
            if (_disposed || string.IsNullOrWhiteSpace(url)) return false;

            try
            {
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute)) return false;

                Debug.WriteLine($"[Definition Plugin] Playing audio from: {url}");

                lock (_lock)
                {
                    if (_disposed || _mediaPlayer == null) return false;

                    var dispatcher = Application.Current?.Dispatcher ?? Dispatcher.CurrentDispatcher;

                    if (dispatcher.CheckAccess())
                    {
                        PlayAudioInternal(url);
                    }
                    else
                    {
                        dispatcher.BeginInvoke(new Action(() => PlayAudioInternal(url)));
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Definition Plugin] PlayAudio Error for {url}: {ex}");
                return false;
            }
        }

        private void PlayAudioInternal(string url)
        {
            try
            {
                if (_disposed || _mediaPlayer == null) return;

                _mediaPlayer.Stop();
                _mediaPlayer.Close();
                _mediaPlayer.Open(new Uri(url));
                _mediaPlayer.Play();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Definition Plugin] PlayAudioInternal Error: {ex}");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                lock (_lock)
                {
                    if (_mediaPlayer != null)
                    {
                        try
                        {
                            var dispatcher = Application.Current?.Dispatcher ?? Dispatcher.CurrentDispatcher;

                            if (dispatcher.CheckAccess())
                            {
                                _mediaPlayer.Stop();
                                _mediaPlayer.Close();
                            }
                            else
                            {
                                dispatcher.Invoke(() =>
                                {
                                    _mediaPlayer.Stop();
                                    _mediaPlayer.Close();
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"[Definition Plugin] AudioManager Dispose Error: {ex}");
                        }
                        finally
                        {
                            _mediaPlayer = null;
                        }
                    }
                }
            }

            _disposed = true;
        }
    }
} 