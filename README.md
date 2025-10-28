프로젝트 개요
-------------------------------------------------------------------------------------------------
이 프로젝트는 ZEP 스타일의 2D 씬에서
플레이어가 자유롭게 이동하며 NPC와 상호작용(E 키)을 통해
미니게임(스택 게임)을 실행하고, 점수/최고점을 저장·관리하는 시스템을 구현한 프로젝트입니다.
-------------------------------------------------------------------------------------------------
[주요 특징]
1. 허브(Hub) 씬<br/>
WASD로 플레이어 이동 가능<br>
NPC와 상호작용 (E 키) → 미니게임 진행<br>
허브 씬은 Tilemap 기반 구조
상호작용 가능한 NPC마다 다른 미니게임 연결 가능<br>
2. 스택(Stack) 미니게임<br/>
좌우로 이동하는 블록을 스페이스바로 정지시켜 쌓기<br>
벗어난 부분은 잘려나감<br>
블록이 맞지 않으면 게임오버<br>
CameraShake + SlowMo + PunchScale + 조각 파티클 효과로 타격감 강화<br>
최고점(PlayerPrefs) 및 최근 점수 저장<br>
3. 점수 시스템<br/>
ScoreManager를 통해 각 미니게임별 최고점/최근점 관리<br>
허브 복귀 시 HubScoreUI가 UI 갱신<br>
4. 씬 전환<br/>
SceneLoader를 통해 허브 ↔ 미니게임 간 전환<br>
마지막 허브 씬 이름을 자동으로 기억 후 복귀 가능<br>
<br>
📁 폴더 구조 (요약) <br/>
Assets/ <br>
&nbsp;&nbsp;Art/<br>
&nbsp;&nbsp;Scenes/ <br>
&nbsp;&nbsp;&nbsp;&nbsp;Hub.unity <br>
&nbsp;&nbsp;&nbsp;&nbsp;Mini_Stack.unity <br>
&nbsp;&nbsp;Scripts/ <br>
&nbsp;&nbsp;&nbsp;&nbsp;Core/           # GameManager, SceneLoader, ScoreManager, HubScoreUI <br>
&nbsp;&nbsp;&nbsp;&nbsp;Player/         # PlayerController2D, CameraFollow2D <br>
&nbsp;&nbsp;&nbsp;&nbsp;Player/Interaction/  # 상호작용 시스템(Interactable, InteractionZome, LoadMiniGameInteractable, NPCStackPortal) <br>
&nbsp;&nbsp;&nbsp;&nbsp;MiniGames/Stack/     # StackGame, StackGame2D, StackBlocks <br>
&nbsp;&nbsp;&nbsp;&nbsp;Effects/        # CameraShake2D, SlowMo, PunchScale, FallAndFade2D <br>
&nbsp;&nbsp;Prefabs/ <br>
&nbsp;&nbsp;&nbsp;&nbsp;Player/         # Player<br>
&nbsp;&nbsp;&nbsp;&nbsp;Stack/          # BcakgroundTile, BaseBlock, BlockPiece, MovingBlock <br>
<br>
5. 핵심 시스템 구조<br>

1.시스템	설명 <br>

PlayerController2D	2D 탑다운 이동 구현 (Rigidbody2D) <br>

InteractionZone	Trigger 영역에서 E키 입력 감지<br>

LoadMiniGameInteractable	미니게임 씬 로드 (SceneLoader.LoadMini()) <br>

SceneLoader	씬 전환 + 허브 복귀 (BackToHub()) <br>

ScoreManager	최고점/최근 점수 관리 (PlayerPrefs) <br>

StackGame2D	스택 게임 로직 + 타격감 효과 통합 <br>

HubScoreUI	허브 씬에서 점수 표시 UI <br>

2.시각적 연출 <br/>

화면 흔들림: 	(CameraShake2D	블록 정지 시 충격 연출} <br>

슬로모션	SlowMo: 	블록 성공/실패 순간 강조 <br>

스케일 펀치 PunchScale:  	블록 크기 변화 강조 <br>

낙하 조각	FallAndFade2D:  	잘려나간 조각이 떨어지며 사라짐 <br>

3.플레이 흐름 <br/>

허브 씬 진입 <br>

NPC 접근 → E키 → “Press E to Play” UI 표시 <br>

미니게임 씬으로 전환 (SceneLoader.LoadMini("Mini_Stack")) <br>

스택 게임 플레이 <br>

게임오버 시 점수 저장 (ScoreManager.SubmitScore("Stack", score)) <br>

Back to Hub 버튼 클릭 → SceneLoader.BackToHub() <br>

허브 씬 복귀 + 점수 UI 갱신 <br>
