# Bouyomi Relay

棒読みちゃんのソケット通信を中継するやつ

<small>(現時点ではCUI版のみです。今後GUI版を作るかもしれません)</small>

## Overview

棒読みちゃんに送信するデータを中継することでプレフィックスとサフィックスを付けることができます。

**Voiceroid Talk Plus** 利用時にプレフィックスを付けることで読み上げキャラクターの使い分けができます。

## Requirement

* [.NET Framework 4.6](https://www.microsoft.com/ja-jp/download/details.aspx?id=48137)

## Installation

0. **Windows 8.1** 以前を利用している場合は[.NET Framework 4.6](https://www.microsoft.com/ja-jp/download/details.aspx?id=48137)をインストール
0. [releases](https://github.com/midorigoke/bouyomi_relay/releases)からダウンロード
0. 展開し任意の場所に配置

## Usage

コマンドプロンプトから

`bouyomi_relay_cui <RX Port> <TX Host> <TX Port> <Massage Prefix> [Massage Suffix]`

ex) `bouyomi_relay_cui 40001 localhost 50001 c)`

(40001ポートで待ち受け、localhostの50001で待ち受けている棒読みちゃんに結月ゆかりで読ませる)

## License

このソフトウェアは[MIT License](https://github.com/midorigoke/bouyomi_relay/blob/master/LICENSE)のもとで公開されています。
