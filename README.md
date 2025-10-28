프로젝트 개요
-------------------------------------------------------------------------------------------------
이 프로젝트는 ZEP 스타일의 2D 씬에서
플레이어가 자유롭게 이동하며 NPC와 상호작용(E 키)을 통해
미니게임(스택 게임)을 실행하고, 점수/최고점을 저장·관리하는 시스템을 구현한 프로젝트입니다.
-------------------------------------------------------------------------------------------------
[주요 특징]
1. 허브(Hub) 씬

WASD로 플레이어 이동 가능

NPC와 상호작용 (E 키) → 미니게임 진입

허브 씬은 Tilemap 기반 구조

상호작용 가능한 NPC마다 다른 미니게임 연결 가능

2. 스택(Stack) 미니게임

좌우로 이동하는 블록을 스페이스바로 정지시켜 쌓기

벗어난 부분은 잘려나감

블록이 맞지 않으면 게임오버

CameraShake + SlowMo + PunchScale + 조각 파티클 효과로 타격감 강화

최고점(PlayerPrefs) 및 최근 점수 저장

3. 점수 시스템

ScoreManager를 통해 각 미니게임별 최고점/최근점 관리

허브 복귀 시 HubScoreUI가 UI 갱신

4. 씬 전환

SceneLoader를 통해 허브 ↔ 미니게임 간 전환

마지막 허브 씬 이름을 자동으로 기억 후 복귀 가능

📁 폴더 구조 (요약)
Assets/
 Art/                 
 Scenes/
  Hub.unity       
  Mini_Stack.unity
 Scripts/
  Core/           # GameManager, SceneLoader, ScoreManager, HubScoreUI
  Player/         # PlayerController2D, CameraFollow2D
  Player/Interaction/  # 상호작용 시스템(Interactable, InteractionZome, LoadMiniGameInteractable, NPCStackPortal)
  MiniGames/Stack/     # StackGame, StackGame2D, StackBlocks
  Effects/        # CameraShake2D, SlowMo, PunchScale, FallAndFade2D
 Prefabs/
  Player/         # Player
  Stack/          # BcakgroundTile, BaseBlock, BlockPiece, MovingBlock

5. 핵심 시스템 구조
{시스템	설명}
PlayerController2D	2D 탑다운 이동 구현 (Rigidbody2D)
InteractionZone	Trigger 영역에서 E키 입력 감지
LoadMiniGameInteractable	미니게임 씬 로드 (SceneLoader.LoadMini())
SceneLoader	씬 전환 + 허브 복귀 (BackToHub())
ScoreManager	최고점/최근 점수 관리 (PlayerPrefs)
StackGame2D	스택 게임 로직 + 타격감 효과 통합
HubScoreUI	허브 씬에서 점수 표시 UI
{시각적 연출}
효과	스크립트	설명
화면 흔들림	CameraShake2D	블록 정지 시 충격 연출
슬로모션	SlowMo	블록 성공/실패 순간 강조
스케일 펀치	PunchScale	블록 크기 변화 강조
낙하 조각	FallAndFade2D	잘려나간 조각이 떨어지며 사라짐
{플레이 흐름}

허브 씬 진입

NPC 접근 → E키 → “Press E to Play” UI 표시

미니게임 씬으로 전환 (SceneLoader.LoadMini("Mini_Stack"))

스택 게임 플레이

게임오버 시 점수 저장 (ScoreManager.SubmitScore("Stack", score))

Back to Hub 버튼 클릭 → SceneLoader.BackToHub()

허브 씬 복귀 + 점수 UI 갱신
