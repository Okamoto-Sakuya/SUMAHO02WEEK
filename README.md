# 3D Action Game（Unity Prototype）

このプロジェクトは、シンプルな3Dアクションゲームのプロトタイプです。  
プレイヤーは左右移動とタップ攻撃を行い、敵を倒してクリアまたはゲームオーバーを目指します。

---

## 操作方法

### PC / スマホ共通
- 画面タップ / 左クリック：攻撃
- Left Button：左へ移動（瞬間移動 + 2秒行動不能）
- Right Button：右へ移動（瞬間移動 + 2秒行動不能）

## ゲームの流れ

1. プレイヤーが左右ボタンで位置移動
2. 画面タップで攻撃
3. 敵にダメージ
4. 敵は1秒後に反撃準備
5. 攻撃コライダーがONになりプレイヤーにダメージ
6. HPが0になるとシーン遷移

## 勝利・敗北条件

### 勝利
- 敵のHPが0になると `ClearScene` に移動

### 敗北
- プレイヤーのHPが0になると `GameOverScene` に移動

## 使用シーン

- GameScene（メインゲーム）
- ClearScene（クリア画面）
- GameOverScene（ゲームオーバー）

※ Build Settings に全シーンを追加してください

## 主なスクリプト

### PlayerController
- 左右移動管理
- 攻撃処理
- クールダウン制御

### PlayerHealth
- HP管理
- ダメージ処理
- ゲームオーバー遷移

### EnemyController
- 反撃タイミング制御
- 攻撃コライダーON/OFF

### EnemyHealth
- HP管理
- ダメージ処理
- クリアシーン遷移

## UI

- SliderでHP表示（Player / Enemy）
- Buttonで左右移動
- Buttonでシーンリスタート可能

## 注意点

- シーン名は完全一致（大文字小文字注意）
- Build Settingsに全シーンを追加すること
- PlayerにはRigidbodyとCollider必須
- EnemyAttackはTrigger Colliderで使用

## 今後の拡張案

- 攻撃アニメーション追加
- ヒットエフェクト
- ノックバック
- 無敵時間（被ダメージ間隔）
- コンボ攻撃システム
- フェード付きシーン遷移

## 作成環境

- Unity 3D
- Input System / Old Input 対応
