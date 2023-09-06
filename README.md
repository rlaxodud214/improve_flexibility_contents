# Flex_Town

<img src="https://user-images.githubusercontent.com/47537803/222512573-50dc0548-dd50-4d6a-8c22-fa8552391c6d.png" width="700" height="400">

> 개발 기간: 2021.12 ~ 2022.12</p>🎥 시연영상 https://www.youtube.com/watch?v=ZRfCsJewmR8&t=1s



## 📃 프로젝트 개요
보건 의료 빅데이터 개방 시스템에 의하면 국내 척추 질환자 수는 연평균 880만 명으로 보고됩니다. 신체 노화에 따른 근력 약화와 좌식생활 증가 등의 이유로 척추 질환을 앓고 있는 환자의 수는 점차 늘어나는 추세입니다.

척추 질환으로 인한 허리 통증은 유연성 감소 및 근육 약화를 초래하여 관절가동범위의 감소로 이어지고 일상생활에 불편함을 겪게 됩니다.

꾸준한 스트레칭은 유연성을 개선해 허리 통증을 감소시킬 수 있으나, 단순히 동작을 반복하는 것은 사용자의 능동적인 참여를 기대하기 어렵습니다. 또한, 유연성 개선 정도 확인을 위해 주로 사용되는 단순 각도계 방식은 정량적인 측정이 어렵습니다.

따라서 본 프로젝트는 관성측정 장치 IMU 센서를 사용하여 실시간으로 측정값을 받아와 흥미 유발 요소를 포함한 유연성 향상 콘텐츠와 유연성의 정량적 측정 및 결과 데이터를 시각화하는 측정 콘텐츠를 제공하는 통합 VR 콘텐츠를 제작하였습니다.

## 🖐프로젝트 소개
사용자는 로그인 시, 통합 콘텐츠에 접속하여 유연성 측정 또는 향상 콘텐츠로 이동할 수 있습니다.

유연성 측정 콘텐츠에서는 총 6가지의 측정 동작(굴곡, 신전, 좌/우측 굴곡, 좌/우측 회전)을 수행합니다. 측정 후, 각 측정값을 합한 종합 유연성에 연령별 유연성 측정지표를 적용하여 데이터를 정량화 및 시각화하여 유연성 분석을 제공합니다.

유연성 향상 콘텐츠는 각 동작을 활용한 5개의 콘텐츠로 이루어져 있습니다. 사용자가 최근 측정한 데이터를 기반으로 임곗값을 설정하여 사용자 맞춤형 콘텐츠를 제공합니다.

## 👨‍👩‍👧팀원
|이름|역할|
|------|---|
|오승연</br>[@O-Wensu](https://github.com/O-Wensu)|- 통합 콘텐츠 개발</br>- 3D콘텐츠 VR 변경 및 통합</br>- 유연성 분석 콘텐츠 개발|
|박소윤</br>[@soun997](https://github.com/soun997)|- 배틀시티 3D콘텐츠 개발</br>- 골키퍼 3D콘텐츠 개발</br>- DB 연동 및 멀티플레이 적용|
|김태영</br>[@rlaxodud214](https://github.com/rlaxodud214)|- 바다표류기 3D콘텐츠 개발</br>- 활쏘기 3D콘텐츠 개발</br>- 유연성 측정 및 분석 콘텐츠 개발|

## 🔨 기술 스택
<img src="https://img.shields.io/badge/Unity-FFFFFF?style=for-the-badge&logo=UNITY&logoColor=black"> <img src="https://img.shields.io/badge/PlayFab-FF6918?style=for-the-badge&logo=PlayFab&logoColor=white"> <img src="https://img.shields.io/badge/Photon PUN2-00427C?style=for-the-badge&logo=PhotonPUN2&logoColor=white">

<img src="https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=GitHub&logoColor=white"> <img src="https://img.shields.io/badge/Notion-FFFFFF?style=for-the-badge&logo=Notion&logoColor=black">

## 💻 기능
+ 통합 콘텐츠
    + 텔레포트, 포탈을 통한 콘텐츠 장소 이동
    + 상점, 인벤토리 기능
    + 펫 뽑기, 펫 보관함에서 조회 및 소환
    + 감정표현을 통한 사용자간 소통
    </p>
+ 유연성 측정 콘텐츠
    + 6가지 동작 기반 유연성 측정
    </p>
+ 유연성 분석 콘텐츠
    + 연령대비 종합유연성
    + 동일 연령대비 종합유연성
    + 자세별 유연성
    + 종합유연성 추세
    + 동작별 유연성 추세
    </p>
+ 유연성 향상 콘텐츠
    + 골키퍼
        + 좌/우측 회전: 좌/우 이동
    + 바다표류기
        + 좌/우측 회전: 좌/우 회전
    + 날아라 슈퍼버드
        + 굴곡/신전: 상/하 이동
    + 배틀시티
        + 굴곡/신전: 상/하 이동
        + 좌/우측 회전: 좌/우 회전
        + 컨트롤러를 사용하여 대포 발사
    + 몬스터 슈터
        + 굴곡/신전: 상/하 시선 이동
        + 좌/우측 회전: 좌/우 회전
    </p>
+ 멀티플레이
