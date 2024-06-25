## 1. Simple Dictionary
```typescript
/**
 * @description The dictionary implementation will be given a list of string.
 * And you will need to write a funciton to find out if the user input is inside the dictionary.
 *
 * input: 'cat', 'car', 'bar'
 *
 * function setup(input: string[])
 * function isInDict(word: string)
 *
 * setup(['cat', 'car', 'bar'])
 * isInDict('cat') // true
 * isInDict('bat') // false
 *
 */

interface Dictionary {
  isInDict(word: string): boolean;
}

class DictionaryImpl implements Dictionary {}
```

Answer: [./BeeInventor.Core/SimpleDictionary.cs](https://github.com/yuijzeon/BeeInventor/blob/master/BeeInventor.Core/SimpleDictionary.cs)

Unit Test: [./BeeInventor.Test/SimpleDictionaryTests.cs](https://github.com/yuijzeon/BeeInventor/blob/master/BeeInventor.Test/SimpleDictionaryTests.cs)

Benchmark 和用 List 實作且資料筆數在 10000 的結果差異:

| Method                                        | Mean       | Error     | StdDev    |
|---------------------------------------------- |-----------:|----------:|----------:|
| find_word_in_list                             | 498.527 ms | 5.6745 ms | 5.3079 ms |
| find_word_in_simple_dictionary                |   1.596 ms | 0.0067 ms | 0.0063 ms |
| find_word_in_simple_dictionary_with_wild_card |  23.098 ms | 0.3989 ms | 0.3537 ms |

Benchmark Source Code: [./BeeInventor.Benchmark/SimpleDictionaryStage.cs](https://github.com/yuijzeon/BeeInventor/blob/master/BeeInventor.Benchmark/SimpleDictionaryStage.cs)


## 2. WildCard Dictionary:
```typescript
/**
 * @description The wildcard dictionary implementation will be given a list of string.
 * And you will need to write a funciton to find out if the user input is inside the dictionary.
 * This implementation is an extended feature for question `Simple Dictionary`
 *
 * input: 'cat', 'car', 'bar'
 *
 * function setup(input: string[])
 * function isInDict(word: string)
 *
 * setup(['cat', 'car', 'bar'])
 * isInDict('cat') // true
 * isInDict('bat') // false
 *
 * WildCard
 * isInDict('*at') // true
 * isInDict('cr*') // false
 *
 */

interface Dictionary {
  isInDict(word: string): boolean;
}
class WildCardDictionaryImpl implements Dictionary {}
```

Answer: [./BeeInventor.Core/WildCardDictionary.cs](https://github.com/yuijzeon/BeeInventor/blob/master/BeeInventor.Core/WildCardDictionary.cs)

Unit Test: [./BeeInventor.Test/WildCardDictionaryTests.cs](https://github.com/yuijzeon/BeeInventor/blob/master/BeeInventor.Test/WildCardDictionaryTests.cs)

Benchmark 和用 List 實作且資料筆數在 100 的結果差異:

| Method                                           | Mean       | Error    | StdDev   |
|------------------------------------------------- |-----------:|---------:|---------:|
| find_wild_card_word_in_list                      | 4,230.1 μs | 58.63 μs | 54.85 μs |
| find_word_in_wild_card_dictionary_with_wild_card |   100.2 μs |  1.95 μs |  3.10 μs |

Benchmark Source Code: [./BeeInventor.Benchmark/WildCardDictionaryStage.cs](https://github.com/yuijzeon/BeeInventor/blob/master/BeeInventor.Benchmark/WildCardDictionaryStage.cs)


## 3. Distributed Dictionary System Design
* Basic Requirements
  1. User can upload texts or books
  2. User can view the texts or books
  3. User should be able to search texts or books

![Distributed Dictionary System Design](https://raw.githubusercontent.com/yuijzeon/BeeInventor/master/BeeInventor.png)

我會選擇使用 ElasticSearch 來做搜尋引擎, 並且使用 File Storage 來存放長文本檔案

1. User can upload texts or books
   - 使用者上傳的長文本檔案會被存到 File Storage (例如 Google Cloud Storage)
   - 相關的 metadata 會被存到 Database (例如 MySQL)
   - 上傳後會透過 Message Queue (例如 RabbitMQ) 來通知後端 File Sync Service 來同步檔案到 ElasticSearch 以供搜尋
      - 這麼做有一定的好處:
         1. 避免太多同時上傳檔案造成 I/O 過高, 程式負載過重等等
         2. 可以避免上傳檔案時造成使用者等待時間過長
         3. 職責分離, 若 File Sync Service 有問題, 不會影響到上傳/檢索檔案的功能 只有長文本搜尋功能會受到影響 (可以服務降級)
         4. 利於擴展, 若 File Sync Service 塞車, 可以增加更多的 File Sync Service 來平行處理 (通常會由 K8S 加上 HPA/KEDA 來根據 metric 自動調整, 也可以先手動執行)
      - 當然也有一定的缺點:
         1. 會增加系統複雜度
         2. 會增加維護成本
         3. 會增加開發成本
         4. 會增加系統延遲
2. User can view the texts or books
   - 使用者可以透過 Application (介面有可能是 Web 或 App) 來瀏覽長文本檔案
   - Application 會返回長文本檔案的 URL 來讓使用者檢視/下載
   - Application 會去 DB 取得長文本檔案的 metadata
3. User should be able to search texts or books
   - 使用者可以透過 Application 來搜尋長文本檔案
   - Application 會透過 ElasticSearch 來搜尋長文本檔案
      1. 搜尋速度快且效果好, 且支援 WildCard 等進階搜尋功能
      2. 基於分散式設計, 提供開箱即用的高可用性
      3. 開源且免費, 同時擁有強大的社群

可能的 DB Schema:

| COLUMN          | TYPE          | CONSTRAINT        |
| --------------- | ------------- | ----------------- |
| FileId          | INT           | PK AUTO_INCREMENT |
| BookId          | INT           | NOT NULL          |
| Path            | NVARCHAR(255) | NOT NULL          |
| IsDeleted       | BIT           | DEFAULT(FALSE)    |
| CreatedBy       | INT           | NOT NULL          |
| CreatedOn       | DATETIME2(3)  | NOT NULL          |
| UpdatedBy       | INT           |                   |
| UpdatedOn       | DATETIME2(3)  |                   |

| COLUMN          | TYPE          | CONSTRAINT        |
| --------------- | ------------- | ----------------- |
| BookId          | INT           | PK AUTO_INCREMENT |
| Title           | NVARCHAR(255) | NOT NULL          |
| Author          | NVARCHAR(255) | NOT NULL          |
| Language        | VARCHAR(20)   |                   |
| IsDeleted       | BIT           | DEFAULT(FALSE)    |
| CreatedBy       | INT           | NOT NULL          |
| CreatedOn       | DATETIME2(3)  | NOT NULL          |
| UpdatedBy       | INT           |                   |
| UpdatedOn       | DATETIME2(3)  |                   |

| COLUMN          | TYPE          | CONSTRAINT        |
| --------------- | ------------- | ----------------- |
| UserId          | INT           | PK AUTO_INCREMENT |
| Username        | VARCHAR(20)   | NOT NULL          |
| Password        | VARCHAR(20)   | NOT NULL          |
| IsDeleted       | BIT           | DEFAULT(FALSE)    |
| CreatedBy       | INT           | NOT NULL          |
| CreatedOn       | DATETIME2(3)  | NOT NULL          |
| UpdatedBy       | INT           |                   |
| UpdatedOn       | DATETIME2(3)  |                   |


> 你可以先從一般學校內部搜尋的系統開始設計，如果還有時間想要補充如何scale out可以再補充描述

如果是一般地方小學或國中 我可能不會做的那麼複雜 一來是成本考量 二來是使用者數量不多 三來是維護難度

如 MQ Service 可能不考慮使用, 直接在上傳後同步到 ElasticSearch, DB 資料使用 memory cache 而非分散式快取快取系統 (例如 Redis), 不使用 K8S 等等