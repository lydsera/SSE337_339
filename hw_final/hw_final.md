# 前言
这是本学期最后一次作业，本文通过粒子系统实现烟花效果

# 相机背景
设置相机背景为黑色，方便显示烟花效果
![在这里插入图片描述](https://img-blog.csdnimg.cn/d2273f9dd7ec4ade881707758098965b.png#pic_center)

# 烟花制作
该粒子系统主要分为两部分，主体部分带动烟花升至空中，另一部分为爆开的烟花，然后两部分都带上环绕的粒子
![在这里插入图片描述](https://img-blog.csdnimg.cn/3120c289c4b1450e8cbbf6c36b0f7708.png#pic_center)
Sub为伴随粒子系统，制作一份复制一份到Burst下
## Fireworks制作
### 基础设置
![在这里插入图片描述](https://img-blog.csdnimg.cn/8e180b835bc749808d3cd7213ab0e67f.png#pic_center)
烟花上升很快，生命周期设置为为1.2，速度设置为55  
颜色设置为红黄两种  

### Emission
![在这里插入图片描述](https://img-blog.csdnimg.cn/7496697dd5cf44079d0cb9752a445798.png#pic_center)
每次发射一枚烟花

### Shape
![在这里插入图片描述](https://img-blog.csdnimg.cn/02b68b5becf245feadc6230eb3643d78.png#pic_center)
发射形状为锥形，角度调小点可以射高点

### Velocity over Lifetime
![在这里插入图片描述](https://img-blog.csdnimg.cn/42a0d8c6bb58421a8e3ec7fe57f1f62c.png#pic_center)
设置粒子速度由1到0模拟烟花速飞升度变慢

### Sub Emitters
![在这里插入图片描述](https://img-blog.csdnimg.cn/cdfe3b4395284c498b630c1987a0c4d7.png#pic_center)


### Trails
![在这里插入图片描述](https://img-blog.csdnimg.cn/b481c042d165483c96a605107d58d3df.png#pic_center)

### Renderer
先制作雪花图片，然后用于制作材质，在renderer中添加材质
![在这里插入图片描述](https://img-blog.csdnimg.cn/0ed0b9ecc52d42b298ad51a34dcead3b.png#pic_center)

## 伴随粒子系统制作
### 基础设置
![在这里插入图片描述](https://img-blog.csdnimg.cn/84b885c4c0254614a1c6b2c081968ecf.png#pic_center)

### Emission&Shape
![在这里插入图片描述](https://img-blog.csdnimg.cn/950dfd4b735344d4a6df8d182aac79a7.png#pic_center)

### Size over Lifetime
![在这里插入图片描述](https://img-blog.csdnimg.cn/a00e6e5430d14a0ba8a5c149be70a096.png#pic_center)
### Rotation over Lifetime&Renderer
![在这里插入图片描述](https://img-blog.csdnimg.cn/2f70060b0daf4d6880e2ef92bbcf28f0.png#pic_center)

## Burst制作
### 基础设置
![在这里插入图片描述](https://img-blog.csdnimg.cn/8877e4e89d5048b6bbf284d1f75ed0ff.png#pic_center)

### Emission&Shape
![在这里插入图片描述](https://img-blog.csdnimg.cn/76630dd0e2ac4c54aad64dbaf6d58342.png#pic_center)

### Size over Lifetime&Sub Emitters&Trails
![在这里插入图片描述](https://img-blog.csdnimg.cn/f6cdec4181044d28a45a0dc4760f65ef.png#pic_center)


### Renderer
同其他粒子一致


## 控制
控制烟花开始停止，变色
```cs
public class Fireworks : MonoBehaviour
{
    // Start is called before the first frame update
    public ParticleSystem[] fireworks;
    int cnt=0;
    int l;
    
    void Start()
    {
        GameObject[] tmp;
        tmp = GameObject.FindGameObjectsWithTag("Fireworks");
        l=tmp.Length;
        fireworks = new ParticleSystem[l];
        
        
        for(int i = 0; i < l; ++i)
        {
            fireworks[i] = tmp[i].GetComponent<ParticleSystem>();
            fireworks[i].Stop();
        }
        

       
    }
    
    
    private void OnGUI() {
        if (GUI.Button(new Rect(Screen.width/2-150, Screen.height-100, 80, 50), "Start")) fireworks[cnt].Play();
        if (GUI.Button(new Rect(Screen.width/2-50, Screen.height-100, 80, 50), "Pause")) fireworks[cnt].Stop();
        if (GUI.Button(new Rect(Screen.width/2+50, Screen.height-100, 80, 50), "Change")){
            fireworks[cnt].Stop();
            cnt++;
            cnt%=l;
            fireworks[cnt].Play();
        }
    }
}
```

# 效果展示
![在这里插入图片描述](https://img-blog.csdnimg.cn/75318ffde71440f18c8a6d0211759803.gif#pic_center)

[演示视频](https://v.youku.com/v_show/id_XNTkzMzQwNzkxNg==.html)


# 参考博客
[unity 制作 粒子 烟花](https://blog.csdn.net/weixin_44373409/article/details/121728387)
