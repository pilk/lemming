using NodeCanvas.Framework.Internal;
using ParadoxNotion.Serialization;
using ParadoxNotion.Serialization.FullSerializer;
using UnityEngine;


namespace NodeCanvas.Framework {

	#if UNITY_EDITOR //handles missing types
	[fsObject(Processor = typeof(fsConnectionProcessor))]
	#endif

	///Base class for connections between nodes in a graph
	[ParadoxNotion.Design.SpoofAOT]
	abstract public partial class Connection {

		[SerializeField]
		private Node _sourceNode;
		[SerializeField]
		private Node _targetNode;
		[SerializeField]
		private bool _isDisabled;
		
		[System.NonSerialized]		
		private Status _status = Status.Resting;

		///The source node of the connection
		public Node sourceNode{
			get {return _sourceNode; }
			protected set {_sourceNode = value;}
		}

		///The target node of the connection
		public Node targetNode{
			get {return _targetNode; }
			protected set {_targetNode = value;}
		}

		///Is the connection active?
		public bool isActive{
			get	{return !_isDisabled;}
			set
			{
				if (!_isDisabled && value == false){
					Reset();
				}
				_isDisabled = !value;
			}
		}

		///The connection status
		public Status status{
			get {return _status;}
			set {_status = value;}
		}

		///The graph this connection belongs to taken from the source node.
		protected Graph graph{
			get {return sourceNode.graph;}
		}


		//required
		public Connection(){}

		///Create a new Connection. Use this for constructor
		public static Connection Create(Node source, Node target, int sourceIndex){
			
			if (source == null || target == null){
				Debug.LogError("Can't Create a Connection without providing Source and Target Nodes");
				return null;
			}

			if (source is MissingNode){
				Debug.LogError("Creating new Connections from a 'MissingNode' is not allowed. Please resolve the MissingNode node first");
				return null;
			}

			var newConnection = (Connection)System.Activator.CreateInstance(source.outConnectionType);

			#if UNITY_EDITOR
			if (!Application.isPlaying){
				UnityEditor.Undo.RecordObject(source.graph, "Create Connection");
			}
			#endif

			newConnection.sourceNode = source;
			newConnection.targetNode = target;
			source.outConnections.Insert(sourceIndex, newConnection);
			target.inConnections.Add(newConnection);
			newConnection.OnValidate(sourceIndex, target.inConnections.IndexOf(newConnection));
			return newConnection;
		}

		///Duplicate the connection providing a new source and target
		public Connection Duplicate(Node newSource, Node newTarget){

			if (newSource == null || newTarget == null){
				Debug.LogError("Can't Duplicate a Connection without providing NewSource and NewTarget Nodes");
				return null;
			}
			
			//deep clone
			var newConnection = JSONSerializer.Deserialize<Connection>(  JSONSerializer.Serialize(typeof(Connection), this)  );

			#if UNITY_EDITOR
			if (!Application.isPlaying){
				UnityEditor.Undo.RecordObject(newSource.graph, "Duplicate Connection");
			}
			#endif

			newConnection.SetSource(newSource, false);
			newConnection.SetTarget(newTarget, false);

			var assignable = this as ITaskAssignable;
			if (assignable != null && assignable.task != null)
				(newConnection as ITaskAssignable).task = assignable.task.Duplicate(newSource.graph);

			newConnection.OnValidate(newSource.outConnections.IndexOf(newConnection), newTarget.inConnections.IndexOf(newConnection));
			return newConnection;
		}

		///Called when the Connection is created, duplicated or otherwise needs validation.
		virtual public void OnValidate(int sourceIndex, int targetIndex){}
		///Called when the connection is destroyed (always through graph.RemoveConnection or when a node is removed through graph.RemoveNode)
		virtual public void OnDestroy(){}

		///Relinks the source node of the connection
		public void SetSource(Node newSource, bool isRelink = true){
			
			#if UNITY_EDITOR
			if (!Application.isPlaying && graph != null){
				UnityEditor.Undo.RecordObject(graph, "Set Source");
			}
			#endif

			if (isRelink){
				var i = sourceNode.outConnections.IndexOf(this);
				sourceNode.OnChildDisconnected(i);
				newSource.OnChildConnected(i);

				sourceNode.outConnections.Remove(this);
			}
			newSource.outConnections.Add(this);

			sourceNode = newSource;			
		}

		///Relinks the target node of the connection
		public void SetTarget(Node newTarget, bool isRelink = true){
			
			#if UNITY_EDITOR
			if (!Application.isPlaying){
				UnityEditor.Undo.RecordObject(graph, "Set Target");
			}
			#endif

			if (isRelink){
				var i = targetNode.inConnections.IndexOf(this);
				targetNode.OnParentDisconnected(i);
				newTarget.OnParentConnected(i);

				targetNode.inConnections.Remove(this);
			}
			newTarget.inConnections.Add(this);

			targetNode = newTarget;
		}


		///////////
		///////////

		///Execute the conneciton for the specified agent and blackboard.
		public Status Execute(Component agent, IBlackboard blackboard){

			if (!isActive){
				return Status.Resting;
			}

			status = targetNode.Execute(agent, blackboard);
			return status;
		}

		///Resets the connection and its targetNode, optionaly recursively
		public void Reset(bool recursively = true){

			if (status == Status.Resting){
				return;
			}

			status = Status.Resting;

			if (recursively){
				targetNode.Reset(recursively);
			}
		}
	}
}