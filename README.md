# ZombiesVsPlants
使用C#编写的经典塔防类游戏《植物大战僵尸》，制作了其中5个植物、4种僵尸、4个关卡。

## 开发环境

开发工具是<code>Visual studio2022</code>,其中用到<code>DirectX SDK</code>当中的<code>Microsoft.DirectX.DirectSound.dll</code>和<code>Microsoft.DirectX.dll</code>。

如果报了这两个引用相关的错误，则需要引用这两个<code>.dll</code>才能够编译运行，这两个<code>.dll</code>文件在项目文件夹里的<code>dependent</code>文件夹内。

以下给出添加这两个引用的过程：

> 1. 用<code>Visual Studio 2022</code>打开项目.
> 2. 在<code>解决方案资源管理器</code>窗口中展开项目，右击<code>引用</code>，点击<code>添加引用</code>.
>    <img src="https://abdusalam-typora.oss-cn-beijing.aliyuncs.com/img-for-typoraimage-20230406024904695.png" alt="image-20230406024904695" style="zoom:33%;" />
> 3. 点击下方的<code>浏览</code>按钮
>    <img src="https://abdusalam-typora.oss-cn-beijing.aliyuncs.com/img-for-typoraimage-20230406025039241.png" alt="image-20230406025039241" style="zoom:33%;" />
> 4. 进入项目目录里的<code>dependent</code>目录，选择里面的两个<code>.dll</code>文件，再点击下方的添加
>    <img src="https://abdusalam-typora.oss-cn-beijing.aliyuncs.com/img-for-typoraimage-20230406025503141.png" alt="image-20230406025503141" style="zoom: 33%;" />
> 5. 最后点击<code>确定</code>即可完成添加依赖
>
> 至此，你就可以编译运行这个项目.



项目调试时可能会报以下异常：
<img src="https://abdusalam-typora.oss-cn-beijing.aliyuncs.com/img-for-typoraimage-20230406034855173.png" alt="image-20230406034855173" style="zoom: 50%;" />

解决方式：
快捷键**Ctrl+Alt+E**，改动<code>Managed Debuggin Assistants</code>的<code>LoaderLock</code>的选中状态去掉即可。



## 项目结构

*项目结构的说明我会在暑假的空闲时间里补充上去。*







