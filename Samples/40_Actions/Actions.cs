﻿namespace Urho
{
	public class _40_Actions : Sample
	{
		public _40_Actions(Context ctx) : base(ctx) { }

		Scene scene;
		Node mushroom;

		public override void Start()
		{
			base.Start();
			CreateScene();
			SimpleCreateInstructionsWithWASD();
			Update += args => SimpleMoveCamera3D(args.TimeStep);
		}
		
		void CreateScene()
		{
			var cache = ResourceCache;
			scene = new Scene(Context);

			scene.CreateComponent<Octree>();
			var planeNode = scene.CreateChild("Plane");
			planeNode.Scale = new Vector3(100, 1, 100);
			var planeObject = planeNode.CreateComponent<StaticModel>();
			planeObject.Model = cache.GetModel("Models/Plane.mdl");
			planeObject.SetMaterial(cache.GetMaterial("Materials/StoneTiled.xml"));

			var lightNode = scene.CreateChild("DirectionalLight");
			lightNode.SetDirection(new Vector3(0.6f, -1.0f, 0.8f));
			var light = lightNode.CreateComponent<Light>();
			light.LightType = LightType.LIGHT_DIRECTIONAL;

			mushroom = scene.CreateChild("Mushroom");
			mushroom.Position = new Vector3(0, 0, 0);
			mushroom.Rotation = new Quaternion(0, 180, 0);
			mushroom.SetScale(2f);

			var mushroomObject = mushroom.CreateComponent<StaticModel>();
			mushroomObject.Model = cache.GetModel("Models/Mushroom.mdl");
			mushroomObject.SetMaterial(cache.GetMaterial("Materials/Mushroom.xml"));

			CameraNode = scene.CreateChild("camera");
			var camera = CameraNode.CreateComponent<Camera>();
			CameraNode.Position = new Vector3(0, 5, -20);
			Renderer.SetViewport(0, new Viewport(Context, scene, camera, null));

			DoActions();
		}

		private async void DoActions()
		{
			MoveBy moveForwardAction = new MoveBy(duration: 1.5f, position: new Vector3(0, 0, 15));
			MoveBy moveRightAction = new MoveBy(duration: 1.5f, position: new Vector3(10, 0, 0));
			ScaleBy makeBiggerAction = new ScaleBy(duration: 1.5f, scale: 3);
			RotateTo rotateYAction = new RotateTo(duration: 2f, deltaAngleX: 0, deltaAngleY: 5, deltaAngleZ: 0);
			MoveTo moveToInitialPositionAction = new MoveTo(duration: 2, position: new Vector3(0, 0, 0));

			await mushroom.RunActionsAsync(moveForwardAction,
				new Parallel(moveRightAction, makeBiggerAction),
				new Parallel(moveToInitialPositionAction, rotateYAction, makeBiggerAction.Reverse()));

			JumpBy jump = new JumpBy(duration: 3, position: new Vector3(10, 0, 0), height: 8, jumps: 3);
			MoveBy moveLeft = new MoveBy(duration: 3, position: new Vector3(-15, 0, 0));
			Blink blink = new Blink(1, 1);

			await mushroom.RunActionsAsync(jump, new EaseElasticOut(moveLeft), new RepeatForever(blink));
		}
	}
}
