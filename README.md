# TakeHomeWork2024_LEESUNJAE

## 내용
    - 게임은 실시간 PvP 형식으로, 2인이 각자의 기기에서 게임에 접속
    - 키보드 화살표를 통해 비행기를 좌우 조작
    - 키보드 스페이스바를 통해 미사일 발사 (0.25sec 쿨타임)
    - 비행기의 HP 3이며, HP는 비행기 옆 원 갯수로 판단
    - 먼저 라이프를 모두 소실 시 패배하며 게임 종료
    - 내부망에서만 테스트를 진행
  
## 환경 설정
    - Unity 2022.3.11f
    - Netcode for GameObject 1.6.0
    - Multiplayer Tools 1.1.1
  
## 소스 수정
1. GameManager
  <img src="docs/GameManager_Inspector.png" width="600" height="100"/>  
  - Player Move Speed: 플레이어 이동 속도
  - Bullet Speed: 미사일 속도
  - Shot Delay: 미사일 발사 쿨타임 (1 = 1초당 1발)
  
## 빌드
  *  Host PC IP Address를 GameScene > NetworkManager > CustomNetworkManager > Address / port 값 변경 후 빌드

## 진행
  * Host PC에선 Host버튼을 같은 내부망 유저는 Client를 선택해서 1:1 대결을 진행합니다.

