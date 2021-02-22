using System.Reflection;
using UnityEditor;
using UnityEngine;
using Type = System.Type;

namespace ScriptForge
{
    [CustomEditor( typeof( ScriptableForge ) )]
    public class ScriptableForgeEditor : Editor
    {
        private bool m_AddForgeButtonSelected;
        private ScriptableForge m_Target;
        private ScriptForgeStyles m_Styles;
        private Vector2 m_ScrollPosition;

        private void OnEnable()
        {
            m_Target = target as ScriptableForge;
        }

        private void OnDisable()
        {
            m_Target.Save();
        }

        private float value;

        /// <summary>
        /// Draws the header for our instance.
        /// </summary>
        protected override void OnHeaderGUI()
        {
            if( m_Styles == null )
            {
                m_Styles = new ScriptForgeStyles();
            }

            GUILayout.Space( 10 );
            GUILayout.BeginHorizontal( "ShurikenEffectBg" );
            {
                GUILayout.Box( GUIContent.none, m_Styles.scriptForgeIconSmall );
                GUILayout.Label( ScriptForgeLabels.HEADER_TITLE, m_Styles.title );
                GUILayout.Label( ScriptForgeLabels.HEADER_SUB_TITLE, m_Styles.subTitle );
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();

            GUILayout.Box( FontAwesomeIcons.INFO, GUIStyle.none );
        }

        public override void OnInspectorGUI()
        {
            float width = Screen.width;
            float height = Screen.height;
            float headerSize = 70;

#if UNITY_5_4_OR_NEWER
            width /= EditorGUIUtility.pixelsPerPoint;
            height /= EditorGUIUtility.pixelsPerPoint;
#endif

            DrawButtons();
            DrawWidgets();
        }

        private void DrawWidgets()
        {
            using( new GUILayout.ScrollViewScope( m_ScrollPosition ) )
            {
                using( new GUILayout.VerticalScope( "GameViewBackground" ) )
                {
                    DrawSpacer();
                    GUILayout.Space( 5.0f );
                    for( int i = 0; i < m_Target.Widgets.Count; i++ )
                    {
                        m_Target.Widgets[i].OnWidgetGUI( m_Styles );

                        DrawSpacer();
                    }

                    GUILayout.FlexibleSpace();
                }
            }
        }

        private void DrawSpacer()
        {
            GUILayout.Space( 5f );
            GUILayout.Box( "", GUILayout.ExpandWidth( true ), GUILayout.Height( 1f ) );
        }

        /// <summary>
        /// Draws the header for script forge.
        /// </summary>
        private void DrawButtons()
        {
            using( new GUILayout.VerticalScope() )
            {
                DrawSpacer();

                if( m_Styles == null )
                    m_Styles = new ScriptForgeStyles();

                if( GUILayout.Button( ScriptForgeLabels.generateAllForgesLabel, m_Styles.button ) )
                {
                    for( int i = 0; i < m_Target.Widgets.Count; i++ )
                    {
                        m_Target.Widgets[i].OnGenerate( false );
                    }
                }

                using( new GUILayout.HorizontalScope() )
                {
                    DrawAddForgeButton();

                    if( GUILayout.Button( ScriptForgeLabels.openAllWidgetsLabel, m_Styles.buttonMiddle ) )
                    {
                        for( int i = 0; i < m_Target.Widgets.Count; i++ )
                        {
                            m_Target.Widgets[i].isOpen = true;
                        }
                    }

                    if( GUILayout.Button( ScriptForgeLabels.closeAllWidgetsLabel, m_Styles.buttonRight ) )
                    {
                        for( int i = 0; i < m_Target.Widgets.Count; i++ )
                        {
                            m_Target.Widgets[i].isOpen = false;
                        }
                    }
                }
                GUILayout.Space( 5.0f );
            }
        }

        /// <summary>
        /// Default buttons can't handle generic menus as a result so we have to make our own buttons. Below
        /// this is how it's done.
        /// </summary>
        private void DrawAddForgeButton()
        {
            Event current = Event.current;
            Rect buttonRect = GUILayoutUtility.GetRect( ScriptForgeLabels.addWidget, m_Styles.buttonLeft );
            int controlID = GUIUtility.GetControlID( ScriptForgeLabels.addWidget, FocusType.Keyboard, buttonRect );

            if( current.type == EventType.MouseDown && buttonRect.Contains( current.mousePosition ) )
            {
                m_AddForgeButtonSelected = true;
                GUIUtility.hotControl = controlID;
                current.Use();
                Repaint();
            }

            if( current.type == EventType.MouseUp )
            {
                m_AddForgeButtonSelected = false;

                if( buttonRect.Contains( current.mousePosition ) && GUIUtility.hotControl == controlID )
                {
                    GenericMenu menu = new GenericMenu();

                    Assembly assembly = Assembly.GetCallingAssembly();
                    Type[] types = assembly.GetTypes();

                    for( int i = 0; i < types.Length; i++ )
                    {
                        if( !types[i].IsAbstract && typeof( Widget ).IsAssignableFrom( types[i] ) )
                        {
                            bool hasInstance = false;

                            for( int x = 0; x < m_Target.Widgets.Count; x++ )
                            {
                                if( m_Target.Widgets[x].GetType() == types[i] )
                                {
                                    hasInstance = true;
                                    break;
                                }
                            }

                            // Get the menu path.
                            string path = types[i].Name;

                            // We don't want to show widgets that are required because they can never be removed.
                            if( System.Attribute.GetCustomAttribute( types[i], typeof( RequiredWidgetAttribute ) ) == null )
                            {
                                if( System.Attribute.GetCustomAttribute( types[i], typeof( InDevelopmentAttribute ) ) != null )
                                {
                                    path = "In Development/" + path;
                                }

                                if( hasInstance )
                                {
                                    menu.AddDisabledItem( new GUIContent( path ) );
                                }
                                else
                                {
                                    menu.AddItem( new GUIContent( path ), false, OnWidgetAdded, types[i] );
                                }
                            }
                        }
                        menu.ShowAsContext();
                    }
                }
            }
            else if( m_AddForgeButtonSelected && current.type == EventType.MouseUp )
            {
                m_AddForgeButtonSelected = false;
                current.Use();
                GUIUtility.hotControl = 0;
                Repaint();
            }

            if( current.type == EventType.Repaint )
            {
                m_Styles.buttonLeft.Draw( buttonRect, ScriptForgeLabels.addWidget, isHover: m_AddForgeButtonSelected, isActive: m_AddForgeButtonSelected, on: false, hasKeyboardFocus: false );
            }
        }

        /// <summary>
        /// Called by our generic menu when a new forge is added.
        /// </summary>
        private void OnWidgetAdded( object widgetType )
        {
            m_Target.OnWidgetAdded( widgetType );
            m_AddForgeButtonSelected = false;
        }
    }
}
