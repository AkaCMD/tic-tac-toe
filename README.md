# tic-tac-toe

这是一个基于 QFramework 开发的 Unity 井字棋游戏项目，支持单人对抗 AI 模式。

## 项目结构

```

Assets/Scripts/
├── Command/              # 命令模式实现各类游戏操作
│   ├── PlaceMarkCommand.cs          # 放置棋子命令
│   ├── ResetAndClearGameCommand.cs  # 重置并清除统计数据命令
│   ├── ResetGameCommand.cs          # 重置游戏命令
│   └── SwitchSidesCommand.cs        # 切换玩家/AI执子命令
├── Controller/           # 控制层处理界面交互逻辑
│   └── GameController.cs            # 主游戏控制器
├── Model/                # 数据模型层
│   ├── GameModel.cs                 # 游戏数据模型实现
│   └── IGameModel.cs                # 游戏数据模型接口
├── System/               # 系统层实现业务逻辑
│   ├── GameRulesSystem.cs           # 游戏规则系统实现
│   └── IGameRulesSystem.cs          # 游戏规则系统接口
├── Utility/              # 工具类
│   ├── IResourceLoader.cs           # 资源加载接口
│   ├── IStorage.cs                  # 存储接口
│   ├── ResourceLoader.cs            # 资源加载实现
│   └── Storage.cs                   # 存储实现
├── GameDefine.cs         # 游戏基础定义（枚举、常量）
├── QFramework.cs         # QFramework框架核心代码
└── TicTacToeApp.cs       # 游戏架构初始化入口
```
## 功能特点

- **MVC 架构**：采用 QFramework 的 MVC 架构模式组织代码
- **命令模式**：使用命令模式处理用户操作和游戏行为
- **数据绑定**：通过 BindableProperty 实现数据与视图的自动同步
- **AI 对战**：内置简单 AI 逻辑，可与玩家对战
- **数据持久化**：使用 PlayerPrefs 保存游戏统计数据
- **资源管理**：使用 Addressables 进行资源加载和管理

## 核心组件说明

### GameModel（数据模型）
管理游戏的核心数据状态，包括：
- 棋盘状态（Board）
- 当前玩家（CurrentPlayer）
- AI 执子方（AIPlayer）
- 游戏结果（Result）
- 分数统计（PlayerScore, AIScore, DrawCount）

### GameRulesSystem（游戏规则系统）
处理游戏核心逻辑：
- 判断游戏胜负和平局
- AI 决策算法（优先获胜>阻止对手>随机）
- 检查指定位置是否为空

### GameController（游戏控制器）
负责 UI 层交互：
- 处理按钮点击事件
- 更新棋盘显示
- 显示玩家信息和分数
- 监听游戏事件并作出响应

### Commands（命令系统）
封装各种游戏行为：
- PlaceMarkCommand：在指定位置放置棋子
- ResetGameCommand：重置当前游戏
- ResetAndClearGameCommand：重置游戏并清空统计数据
- SwitchSidesCommand：切换玩家与AI的执子角色

## 技术亮点

1. **架构清晰**：遵循 QFramework 的架构设计，各层职责分明
2. **易于扩展**：模块化设计便于添加新功能
3. **数据驱动**：使用 BindableProperty 实现响应式数据更新
4. **事件系统**：通过事件机制解耦组件间通信
5. **AI 实现**：具备基础AI能力，可根据需要增强算法复杂度

## 使用说明

1. 点击棋盘空位放置棋子
2. "Reset" 按钮重置当前游戏
3. "Reverse" 按钮切换玩家与AI的执子角色
4. 游戏会自动记录玩家、AI胜利次数和平局次数

## 开发环境

- Unity 2022.3.51f1
- QFramework 框架（已内置于项目中）
- Addressables 资源管理系统
```
