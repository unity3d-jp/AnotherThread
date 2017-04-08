# AnotherThread
60fps(PS Vitaの実績)で動作するシューティングゲームのデモです。
モバイル・デスクトップ・コンソールいずれも動作しますが、WebGLでは動作しません。
ゲームロジックを別スレッドで記述したサンプルです。Unite 2016 Tokyo の講演のために作りました。

This project is a demo for 60fps(on PS Vita).
It works with mobile, desktop, and console. Except WebGL.
This is a sample of game logic with another thread. Published at Unite 2016 Tokyo.

![](http://i.imgur.com/9EF4y3d.png)

Presentation slide
[http://japan.unity3d.com/unite/unite2016/files/DAY1_1700_room1_Yasuhara.pdf]

Presentation Video
[https://www.youtube.com/watch?v=VNVDtUT_4rs&index=1&list=PLFw9ryLdiLza_ORQFwfPyOSTXFcD7EB6J]


----
HUDのテクスチャが壊れていた場合は、
Window->AtlasExporter
から
"convert atlas"
を実行してください。

In case HUD textures are corrupted,
Please execute
Window->AtlasExporter
"convert atlas"



----
これは技術サンプルです。
ゲーム部分は、かなりいい加減に作られているのでご注意ください。

This is a "tech-sample" project.
Please note that the game itself is NOT well designed.
