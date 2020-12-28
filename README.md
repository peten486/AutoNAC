# AutoNAC
WI-FI 와 랜선 연결시에 사내 NAC 인증을 자동으로 하도록 하는 프로그램


## 1. 개발 환경
   .NET Form / Selenium based **C#**

## 2. 개발 순서
 1) Network 연결( WI-FI / 랜선 ) 개발 : 랜선을 끼우면 랜선이 주가 되고 WI-FI 연결을 해재, 랜선을 뽑으면 WI-FI에 자동으로 연결되게끔
  - WI-FI 연결 / 연결 해제(OK)
  - WI-FI 자동 연결(X)
 2) Selenium을 통한 NAC 페이지 자동 인증 개발 : localhost에 NAC 페이지와 유사한 페이지를 두고 개발
  - Selenium으로 페이지 인증(OK)
 3) C# Controller Class에 1)과 2)의 Work을 하는 Thread를 개발, 공유자원 처리 확인
  - Worker thread 개발
  - 공유자원인 현재 internet 연결 정보 개발
 4) CUI 기반으로 테스트
 5) 마지막 GUI Form 개발
 6) Test

## 3. 관련 기술
 Selenium
 
 Edge Brower
