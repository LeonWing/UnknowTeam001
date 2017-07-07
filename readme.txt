--------------------------------------------------------------------------------------
Project: PaperPlane
--------------------------------------------------------------------------------------
Versions: 0.00.001
--------------------------------------------------------------------------------------
设定初始值：
-------------------------
Hierarchy.PaperPlane
Position(-30,30,-15);Rotation(-130,0,-90);Scale(3,3,3)
-------------------------
Block.prefab
Position(0,0,-20);Rotation(0,0,0);Scale(50,3,10)
--------------------------------------------------------------------------------------
动态设定：
-------------------------
Hierarchy.Canvas.score
显示当前层数
-------------------------
Block.prefab
轮流调用 Position(0,0,-20)&Position(0,0,20)
-------------------------
LayerHeight
两个Block之间Y轴间距=40

=========================
2017/6/30	Tnias
增加了纸飞机自转
由于只推送了PaperPlaneBehaviour.cs文件，unity内部参数均未上传，因此，在测试运行时请将自转速度（Self Rotation Speed）调整为2。。。
=========================
2017/7/6	Tnias
增加了GameManger.cs用来控制整个游戏，其中包括Block的生成（目前按照左右间隔生成方式，脚本模式暂未实现）
飞机增加了移动功能及单位速度参数，其中包括x、y分量
在BlockController.cs中添加了Block的移动过程和销毁过程，其中增加了distance变量用于标记该Block是在多少距离时生成的，增加了p变量用于标记该Block位于屏幕的哪一边。

关于新的公共参数说明：

PaperPlaneBehaviour.cs：
Rotate Speed：转向速度
SelfRotateSpeed：自转速度
RotationLimite：飞机转向角限制参数，有效值为0-80，表示飞机可以转角的度数
VerticalWidth：表示飞机垂直下降的半角范围（即飞机不水平移动），5表示+5到-5度之间垂直坠落

GameManager.cs：
FallSpeed：下落速度调整值
HorizontalSpeed：水平速度调整值
HorizontalBoundary：飞机位移边界（距中心点）
ScreenHorizontalBoundary：屏幕水平边界，默认45，距中心点
ScreeVerticalBoundary：屏幕竖直边界，默认80，距中心点
BlockDistance：Block的默认间距
GameObject：Block资源，需手动绑定至Assets/Models/Block
===========================
2017/7/7
修改HorizontalSpeed，启用sin算法
修改碰撞规则
PS：游戏难度简直了
===========================