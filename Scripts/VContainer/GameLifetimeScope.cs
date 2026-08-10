// using VContainer;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using VContainer.Unity;
//
// public class GameLifetimeScope : LifetimeScope
// {
//     [SerializeField] private PlayerController player;
//     protected override void Configure(IContainerBuilder builder)
//     {
//         builder.RegisterComponent(player);
//         builder.Register<PoolManager>(Lifetime.Singleton);
//         builder.Register<ResourceManager>(Lifetime.Singleton);
//     }
// }
