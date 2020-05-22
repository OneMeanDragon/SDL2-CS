Imports SDL_VB.SDL2

Module MainMod

    Public Sub Main()
        SDL.SDL_Init(SDL.SDL_INIT_VIDEO)
        Dim window As IntPtr = IntPtr.Zero
        window = SDL.SDL_CreateWindow("Title", SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED, 1020, 800, SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE)
        SDL.SDL_Delay(5000)

        'Dim m_event As SDL_Event '<-- broke lol might look at it again but doubtful

        Dim quit As Boolean = False

        While (Not quit) 'to break out set quit to true in the debugger
            'While SDL.SDL_PollEvent(m_event)

            'End While
        End While

        SDL.SDL_DestroyWindow(window)
        SDL.SDL_Quit()
    End Sub

End Module
