﻿## 资源

### 定义
游戏引擎所依赖的外部文件统称为游戏的资源。<br/>
在Yuri Engine中，游戏资源一共有三种：**图像**资源、**音声**资源、**脚本**资源。

### 游戏的目录结构
- 根目录 
  - PictureAssets （规格：PNG、JPG、BMP）
      - background （背景）
      - character （角色立绘）
      - pictures （图片）
  -  Sound（规格：MP3、WAV）
      - bgm （背景音乐）
      - bgs （背景音效）
      - se （音效）
      - vocal （语音）
  -  Scenario （逻辑脚本）

资源必须被放置在正确的目录下才能被引擎正确地引用。

### 资源的封包
Yuri引擎能够将资源文件按照不同的类别打包成一个文件，以缩小游戏尺寸，提高访问速度。Yuri引擎的资源打包在可视化开发环境Halation中进行，其使用方式请参考**用户手册**，此处只讨论其技术细节。<br/>
不同文件夹下的资源被分别打包成一个文件，打包过程如下：

- 扫描一遍该目录下的文件列表并记录
- 将每一个文件的二进制数据写入到缓冲区中，并将该文件的名字、在缓冲区中的起始位置指针、字节长度记录记录在一个字典中
- 将该缓冲区写到稳定储存器上，作为封包文件
- 将字典写到稳定储存器上，与封包文件同名，但后缀名是.PST，该文件作为封包数据的索引

在游戏运行时，引擎将自动解析PST文件并在需要访问资源的时候通过索引中的指针读取相应的资源。在封包文件中找不到相应的资源后，引擎将继续搜索是否在文件夹中存在未封包的资源文件。如果都未找到，说明游戏文件残缺或逻辑脚本错误，引擎将**抛出错误**并强制结束游戏。<br/>
Yuri引擎的只实现了资源的打包，并未实现资源的加密，如果需要实现加密功能，请修改`Yuri.Utils.PackageUtils`中的打包、解包方法。

### 资源缓存
Yuri Engine在运行时会在内存中缓存最常使用的资源文件的字节流，以避免频繁的稳定储存器IO开销。最大缓存数目可以通过修改配置文件来更改，引擎默认缓存运行时使用次数前一百位的图像资源。 <br/>
资源缓存的策略是**最近最常使用原则**，当资源每次被使用时，其引用计数会递增。资源缓冲池管理器`Yuri.PlatformCore.VM.ResourceCachePool`中维护着一个**大根堆**，它维持着前一百个最近最常使用资源在托管内存中的字节块避免其被CLR垃圾回收。一旦游戏逻辑脚本需要引用一个资源，资源管理器`ResourceManager`会先在资源缓冲池管理器`ResourceCachePool`中检索该资源是否存在大根堆中，如果存在，那么直接通过其引用维持指针取出字节流供上层调用者使用；如果不存在，则更新缓冲池中资源引用计数字典，并从文件中读取字节流给上层，并检查该资源的引用计数是否已经拥有缓存的资格，如果是，那么将该记录插入大根堆并调整堆以维持性质以达到缓存功能。丧失缓存资格的资源，缓冲池管理器将**悬空**它的托管内存引用指针，等待CLR进行垃圾回收。<br/>
事实上，极少数资源并不会被插入大根堆参与缓存竞争，而是被提升为**持久缓存**，在游戏期间总是不被垃圾回收，例如对话框底图等。关于内存管理相关技术细节，详见“**运行时环境**”章节。

### 程序集信息
| Property | Value |
| :-------- | :--------: |
| 层次结构   | Yuri.PlatformCore.ResourceManager |
| 最低版本   | 1.0 |
| 并行安全   | 是 |

| Property | Value |
| :-------- | :--------: |
| 层次结构   | Yuri.Utils.PackageUtils |
| 最低版本   | 1.0 |
| 并行安全   | 读安全，写不安全 |

| Property | Value |
| :-------- | :--------: |
| 层次结构   | Yuri.PlatformCore.VM.ResourceCachePool |
| 最低版本   | 1.0 |
| 并行安全   | 是 |