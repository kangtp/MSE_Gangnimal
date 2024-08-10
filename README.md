
<h2> ⚔️ 3D PVP 슈팅 게임 : Gangnimal ⚔️</h2>
Gangnimal은 귀여운 동물들이 무기와 아이템을 주워 던지며 싸우는 실시간 3D TPS 게임입니다!<br> 맵에 랜덤으로 떨어져 있는 아이템과 무기를 파밍해 적에게 던져 피해를 입히세요! <br> 귀여운 여러가지 동물 캐릭터가 있어 남녀노소 재밌게 즐길 수 있습니다! 🫠

### ▶️[게임 트레일러](https://youtu.be/SpQS2xCl5lI?si=qg6oC5v5u5Hg0fzY)

## 목차
  - [개요](#개요) 
  - [게임 설명](#게임-설명)
  - [주요 기능](#주요-기능)

## 개요
- **프로젝트 이름**: Gangnimal 🍡
- **개발 기간**: 2024.03-2024.06
- **사용 기술 스택, 언어**: Unity,C#(클라이언트), Java,Spring(서버)
- **팀 구성**: 강현서, 박선준 / 양경덕, 우지수 (서버 개발 2 / 클라이언트 개발 2)

## 게임 설명

### **게임 특징**
- **캐릭터 선택**: 플레이어는 각각 고유의 능력을 가진 곰, 말, 토끼 중 하나를 선택할 수 있습니다.

|곰|말|토끼|
|:------:|:---:|:---:|
|![image](https://github.com/user-attachments/assets/110194e2-9e5e-4b38-9fd2-8027a579b56f)|![image](https://github.com/user-attachments/assets/06b58bac-58b6-470f-acb7-21dde0be5bdf)|![image](https://github.com/user-attachments/assets/c3ccff9b-9c30-42f8-900e-d9230e2ccfa2)|
|체력 높음|이동속도 빠름|점프력 높음|
<br/>

- **맵 선택**: 세 가지 테마의 맵(숲, 사막, 겨울)이 있으며, 게임 시작 전에 호스트 플레이어가 맵을 선택합니다.

|숲속|사막|겨울|
|:------:|:---:|:---:|
|![image](https://github.com/user-attachments/assets/d6610323-b8cd-450e-bf5b-de92b7ef050f)|![image](https://github.com/user-attachments/assets/69dfd49a-1344-4678-97fb-1e5b8875e85b)|![image](https://github.com/user-attachments/assets/97f1be32-c268-4c06-80e5-906e4a825c2e)|
<br/>

- **무기 및 아이템**:
  - 플레이어는 맵에 흩어져 있는 무기와 아이템을 획득할 수 있습니다.
  - 무기는 E키를 눌러 주울 수 있고, 힐팩과 방패 아이템은 충돌 시 자동으로 획득되어 HP를 증가시키거나 쉴드를 제공합니다.

|나뭇가지|돌|폭탄|쉴드|힐팩|
|:------:|:---:|:---:|:---:|:---:|
|![image](https://github.com/user-attachments/assets/652f743c-6f45-47b9-bc2b-13aa11309eee)|![image](https://github.com/user-attachments/assets/44e60465-9617-4143-b328-91cabcd1d2b2)|![image](https://github.com/user-attachments/assets/bdd729f5-b49c-431f-a651-a70c9fa29b03)|![image](https://github.com/user-attachments/assets/37def79f-8149-4355-9af4-97fac1caaf1a)|![image](https://github.com/user-attachments/assets/a0047183-11f2-47af-a9b2-cd1a65b7938d)|
|데미지 10|데미지 20|데미지 30|공격 1회 방어|체력 +10|
<br/>


- **탄도 시스템**: 힘을 조절하여 발사각도를 조절할 수 있는 탄도 시스템을 적용했습니다.
<br/><br/>
![제목 없는 동영상 - Clipchamp로 제작](https://github.com/user-attachments/assets/ab57fe2c-d23b-41cc-b8ac-b5f559a96db5)

<br/>

## 주요 기능

### 1. Spring과 UnityWebRequest를 이용한 서버 연동
**Spring** 프레임워크를 이용해 로컬 서버를 구축하고, UnityWebRequest를 통해 로컬 서버와 클라이언트를 연결합니다.<br/><br/>
**회원가입 및 로그인 기능**
|![image](https://github.com/user-attachments/assets/d9333361-82ab-45ec-a8ce-1cdf8474387b)|![image](https://github.com/user-attachments/assets/edf54c76-ec64-4418-a7aa-2f251ad2ee4b)|
|:---:|:---:|
|로그인 화면|회원가입 화면|
- **회원가입** : Unity 클라이언트에서 계정을 생성하면 계정 ID와 Password를 UnityWebRequest를 통해 서버로 전송하고, 동일한 ID의 계정이 존재하지 않으면 해당 계정을 JPA Repository(DB)에 저장합니다. 동일한 ID가 존재한다면 실패합니다.
- **로그인** : Unity 클라이언트에서 아이디와 패스워드를 입력합니다. UnityWebRequest로 해당 정보를 서버로 전송하고, DB에 해당 계정 정보가 존재하면 성공, 없으면 실패합니다.

**전적 기록 시스템**
<br/><br/>
![image](https://github.com/user-attachments/assets/f5998166-25e6-400e-94c9-c13ffd30424f)

- PVP가 끝나면 승패에 따라 정보를 update하고 서버로 전송합니다.
- 메인화면의 record 버튼을 누르면 서버로 자신의 승패 정보를 GET Request를 통해 요청하게 되고, UI를 통해 자신의 승패 수, 승률을 확인할 수 있습니다.

<br/>

### 2. Unity Relay를 이용한 Lobby 시스템 구현
- unity에서 지원하는 Lobby system 라이브러리를 사용하여서 매칭 시스템을 구현하였습니다.
- 처음 방을 만들게 되면 그사람은 Host가 되어서 다른 플레이어가 로비에 접속할때까지 기다립니다. 그리고 joinCode를 입력한 다른 플레이어는 로비에 접속하게 되고 Client로 지정해줍니다.
- 로비에 접속한 Host와 Client를 동시에 게임을 시작해주기 위해서는 Unity에서 지원하는 Relay를 사용하여서 같은 GameScene안에 Host와 Client Prefab을 생성해주었습니다. 이렇게 해서 각 플레이어는 게임을 시작할 준비가 되었습니다.

<br/>

### 3. Unity Netcode로 오브젝트 동기화
- Unity에서 지원하는 Netcode 라이브러리를 사용해 호스트와 클라이언트 간의 오브젝트 동기화를 구현했습니다.
- Client의 데이터에 변화가 있을 때(오브젝트의 transform, HP의 변화, VFX 등) Client에서 ServerRpc를 호출해 Server(호스트)에서 Client의 데이터를 수정하도록 했습니다.
- 오브젝트가 삭제되어야 할 때, 서버에서 despawn을 해주어 클라이언트에도 동일하게 해당 오브젝트가 없어지도록 했습니다.

<br/>

### 4. 적용된 디자인 패턴
- **싱글톤 패턴**: 게임 상태와 사용자 정보를 관리하는 데 사용됩니다.
- **옵저버 패턴**: 게임 중 플레이어의 체력을 효율적으로 업데이트하는 데 사용됩니다.
