# Why another MP3 player?

I've been wanting to learn XAML/WPF for a long time, *and* there are some features of MP3 players that I've been wanting. Most of the work by far has been done by [Sabapathy](https://github.com/sabapathygithub), and I've been learning from his contributions. I still find XAML and its MVVM architecture kind of complicated. Oh well.

Play Next vs Play at End. Sometimes I want a song to play immediately next, sometimes at the end of the current queue.

![image](https://user-images.githubusercontent.com/4549398/126873681-167aa700-ea63-498b-ae4f-871e51e47523.png)

I also want more control over the metadata loading and editing via [MetadataCache](https://github.com/adamfoneil/MyMusic.Wpf/blob/master/MyMusic.Wpf/Services/MetadataCache.cs). I have a fast load time (by my reckoning) once the cache is built:

![image](https://user-images.githubusercontent.com/4549398/126873791-fc6ff120-6186-4ae5-a347-747006ad70b1.png)

Main screens:

![img](https://adamosoftware.blob.core.windows.net/images/E1J6COWOWR.png)

![image](https://user-images.githubusercontent.com/4549398/126872934-8ed79800-ff72-48d1-bc75-10cfa0a29b30.png)
