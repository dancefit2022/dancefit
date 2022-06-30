// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  //识别的骨骼输出控制器
  public class PoseLandmarkListAnnotationController : AnnotationController<PoseLandmarkListAnnotation> {
    [SerializeField]
    private bool _visualizeZ = false;

    private IList<NormalizedLandmark> _currentTarget;
    //获取
    public Action<IList<NormalizedLandmark>> onSyncNowEnd;
    public void DrawNow (IList<NormalizedLandmark> target) {
      _currentTarget = target;
      SyncNow ();
    }

    public void DrawNow (NormalizedLandmarkList target) {
      DrawNow (target?.Landmark);
    }
    //设定NormalizedLandmark 数据 lateupdate 时 在AnnotationController 调用 SyncNow
    public void DrawLater (IList<NormalizedLandmark> target) {
      UpdateCurrentTarget (target, ref _currentTarget);
    }

    public void DrawLater (NormalizedLandmarkList target) {
      DrawLater (target?.Landmark);
    }

    protected override void SyncNow () {
      isStale = false;
      annotation.Draw (_currentTarget, _visualizeZ);
    }

    //抛出 sync 后的事件
    protected override void LateUpdate () {
      if (isStale) {
        SyncNow ();
        if (onSyncNowEnd != null && _currentTarget != null) {
          onSyncNowEnd (_currentTarget);
        }
      }
    }
  }
}
