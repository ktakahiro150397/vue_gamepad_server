# システム構成

- 本リポジトリのサーバープログラムでゲームパッドの入力を取得する
- サーバーはエンドポイント`/GetInputStream`を公開し、ゲームパッドの入力情報を送信する
    - 送信フォーマットはServer Sent Event(SSE)
- 入力されていた時間をあわせて送信する。
    - サーバーのティック数単位で送信する。60Hzを想定している。
- 送信されたデータは`vue_gamepad_display`で表示する
    - https://github.com/ktakahiro150397/vue_gamepad_display
    - 対応するボタンのバインド・アイコンの設定は表示側で行う


![構成図](./vue_gamepad_server.drawio.png)

# タスク

- [x] ゲームパッド入力の取得処理調査ごと
    - [x] サーバープログラムで直接Win32 APIを利用して取得できるかどうか？
        - できる。60Hz制限でポーリングし、キー入力に変更があれば送信する。
        - 方向キーを`dwPos`の角度から判定する。
        - ボタンは`dwButtons`のフラグで判定する。ボタンのインデックスはブラウザ側のGamepad APIと一致しているように見える。
    - [x] フロントで使用しているGamepad APIと同等の入力取得ができるかどうか？
        - 上記の通りで、多分大丈夫。
- [x] API動作確認ごと
    - [x] `TaskCompletionSource<T>`の動作確認
        - パッドボタン押下のイベントを受けとり、そのハンドラの中でデータを突っ込んで送信するイメージ？
        - https://medium.com/the-pragmatic-tech-review/asynchronous-programming-in-net-understanding-taskcompletionsource-599c6fe47537
        - これは不要でした。
- [x] 送信スキーマ設計
    - フロントで必要な項目のみ含めて送信する
    - ボタン押下はGamepad APIの形式に変換して送信するとフロントで楽ちん


# 送信データスキーマ

## /GetInputStream

`/GetInputStream`でフロント側に送る送信データのスキーマ。

### リクエスト

|項目名|型|値|
|--|--|--|
|joyId|int|入力情報を受け付けるパッドのIDを指定する。`/GetDevices`一覧で返却された`joyId`を指定する。|

### レスポンス

|項目名|型|値|
|--|--|--|
|button_state|[bool,bool...]|ボタン1-16までの押下ステータス。ボタンが押下されている場合、`true`。押下されていない場合、`false`。値のインデックスはGamepad APIのインデックスと対応している。|
|direction_state|int|方向キーの入力方向押下ステータス。テンキー表記に従って、整数で返却する。(4:左・6:右・5:ニュートラル)|
|time_stamp|int|ボタン入力を検知した時点のエポック秒。(UTC基準)|
|previous_push_frame|int|直前の入力が継続していたサーバーのティック数。初回の場合、`0`。最大値は`99`。|

 - データ取得OKの場合、200ステータスで返却
 - エラー発生時は500ステータスで返却
    - フロント側では、ステータスコードを取得する

## /GetDevices

Windows本体に接続されているゲームパッドの情報を配列で返却する。
このレスポンスに含まれる`joyId`を`/GetInputStream`のクエリパラメータに指定する必要がある。
合わせて、サーバーの情報も送信しておく。

|項目名|型|値|
|--|--|--|
|joyId|int|接続されているゲームパッドの、Windows側で割り振られているID。|
|device_id|string|デバイスごとの固有のID(たぶん)。|
|device_name|string|画面に表示するデバイスの名称。|
|server_tick|int|入力時間の計測に使用するサーバーのティック数。`60`を想定。|