# 플랫포머용 A* 알고리즘
플랫포머에서 사용 가능한 A* 알고리즘입니다.
플랫포머 게임에서 적용되기 위해 중력을 고려하여 경로를 탐색합니다.

Hierarchy 상의 각 오브젝트의 역할은 다음과 같습니다.

####1. Grid : 맵과 경로를 그리는 타일의 그리드입니다. 타일의 크기는 0.32 x 0.32 (32pixels x 32pixels) 입니다.
    1.A MapTile : 맵을 그리는 타일입니다. 기본적으로 테스트 맵이 그려져 있습니다. 필요에 따라 직접 맵을 그리거나 지워서 테스트하실 수 있습니다.
    1.B Path : 경로를 그리는 타일입니다. 추가적인 조작의 필요는 없습니다.

####2. Start : A* 탐색을 시작하는 오브젝트입니다. MapTile의 범위 안에 존재해야 하며, MapTile에 이미 그려진 타일 위에 존재할 경우 벽에 끼인것으로 간주, 경로를 탐색하지 않습니다.

####3. Target : A* 탐색의 목표인 오브젝트입니다. MapTile의 범위 안에 존재해야 하며, MapTile에 이미 그려진 타일 위에 존재할 경우 벽에 끼인것으로 간주, 경로를 탐색하지 않습니다.

####4. Reset : 리셋 버튼입니다. 클릭 시 이미 찾은 경로를 지웁니다.

####5. Research : 재탐색 버튼입니다. 클릭 시 경로를 재탐색 합니다.

####6. Event System : 버튼 입력 이벤트를 처리하는 유니티 기본 객체입니다. 추가로 버튼의 동작또한 정의합니다.


## AStarPathFinder
A* 알고리즘을 실행하는 객체입니다. 

### AstarPathFinder(int [,] grid, int xMin, int yMin)
객체를 생성할 때 탐색을 적용할 맵의 정보 -grid- 와 해당 맵의 Tilemap cellbounds 데이터를 입력해줍니다. 

### FindPathInField(Vector2Int start, Vector2Int target, int jumpForce = 1)
타일맵 구조의 맵에서 탐색을 시작합니다. 시작점과 도착점의 tilemap상 좌표를 입력받고, [-inf, inf] 의 값을 [0, inf]의 형태로 변환 후 A* 알고리즘을 실행합니다.
이 때 jumpForce는 뛰어 넘을 수 있는 y블럭의 칸 수 입니다.

## PathFindObject
탐색을 실행하는 객체입니다. Start, Target이 components로 가집니다.
각 멤버의 역할은 다음과 같습니다.
1. currentMap : 현재의 Map
2. tileMapPos : 객체의 Tilemap에서의 위치
3. targetObject : 목표지점의 오브젝트
4. findPath : 경로를 찾았음을 나타내는 토글 변수
5. tilemap : 경로를 출력하는 타일맵
6. pathTile : 경로를 출력하는 룰타일
7. jumpF : 최대 점프 가능 높이

### Update
매 프레임마다 현재 위치를 기반으로 tile map 에서의 좌표로 변환하여 tileMapPos에 저장함.
findPath 토글이 비활성화 되어있으면, A* 알고리즘을 실행, 경로를 탐색 후 해당 토글을 활성화
경로를 찾으면 해당 경로를 Debug로 출력 및 시각적으로 그림

### IsObjectOnTilemap ()
객체가 tilemap의 범위 안에 있는지 검사하는 메소드. tilemap의 범위 안에 있더라도 막혀있는 타일의 경우 false를 반환.

## Map
Grid를 가지는 맵입니다. 타일의 grid를 자식 오브젝트로 가집니다.
각 멤버들의 역할은 다음과 같습니다.
1. mapTile : 적용되는 타일맵입니다.
2. mapGridData : mapTile의 타일 정보를 grid Data화 하여 저장한 값입니다.
3. aStar : aStar 객체입니다.

### ConvertTilemapToGrid()
타일맵의 정보를 2차원 배열로 저장하는 메소드입니다. [-inf, inf] 의 값을 가지는 tilemap상 좌표를 [0, inf]의 형태로 변환하여 저장합니다.
