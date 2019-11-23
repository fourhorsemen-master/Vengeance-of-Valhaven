using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ControlMatrix
{
    public static CastingCommand GetCastingCommand(CastingStatus previousStatus, ActionControlState previousControl, ActionControlState currentControl)
    {
        var layer = _controlMatrix[previousStatus];
        var row = layer[previousControl];

        return row[currentControl];
    }

    private static Dictionary<CastingStatus, Layer> _controlMatrix = new Dictionary<CastingStatus, Layer>
    {
        {
            CastingStatus.ChannelingLeft,
            new Layer(
                new Row(
                    CastingCommand.ContinueChannel,
                    CastingCommand.CancelChannel,
                    CastingCommand.ContinueChannel,
                    CastingCommand.CancelChannel
                ),
                null,
                new Row(
                    CastingCommand.ContinueChannel,
                    CastingCommand.CancelChannel,
                    CastingCommand.ContinueChannel,
                    CastingCommand.CancelChannel
                ),
                null
            )
        },
        {
            CastingStatus.ChannelingRight,
            new Layer(
                null,
                new Row(
                    CastingCommand.CancelChannel,
                    CastingCommand.ContinueChannel,
                    CastingCommand.ContinueChannel,
                    CastingCommand.CancelChannel
                ),
                new Row(
                    CastingCommand.CancelChannel,
                    CastingCommand.ContinueChannel,
                    CastingCommand.ContinueChannel,
                    CastingCommand.CancelChannel
                ),
                null
            )
        },
        {
            CastingStatus.Cooldown,
            new Layer(
                new Row(CastingCommand.None),
                new Row(CastingCommand.None),
                new Row(CastingCommand.None),
                new Row(CastingCommand.None)
            )
        },
        {
            CastingStatus.Ready,
            new Layer(
                new Row(
                    CastingCommand.None,
                    CastingCommand.CastRight,
                    CastingCommand.CastRight,
                    CastingCommand.None
                ),
                new Row(
                    CastingCommand.CastLeft,
                    CastingCommand.None,
                    CastingCommand.CastLeft,
                    CastingCommand.None
                ),
                new Row(
                    CastingCommand.None
                ),
                new Row(
                    CastingCommand.CastLeft,
                    CastingCommand.CastRight,
                    CastingCommand.CastLeft,
                    CastingCommand.None
                )
            )
        }
    };

    internal class Layer : Dictionary<ActionControlState, Row>
    {
        internal Layer(
            Row leftLastFrame,
            Row rightLastFrame,
            Row bothLastFrame,
            Row noneLastFrame
        )
        {
            this[ActionControlState.Left] = leftLastFrame;
            this[ActionControlState.Right] = rightLastFrame;
            this[ActionControlState.Both] = bothLastFrame;
            this[ActionControlState.None] = noneLastFrame;
        }
    }

    internal class Row : Dictionary<ActionControlState, CastingCommand> {
        internal Row(
            CastingCommand leftThisFrame,
            CastingCommand rightThisFrame,
            CastingCommand bothThisFrame,
            CastingCommand noneThisFrame
        )
        {
            this[ActionControlState.Left] = leftThisFrame;
            this[ActionControlState.Right] = rightThisFrame;
            this[ActionControlState.Both] = bothThisFrame;
            this[ActionControlState.None] = noneThisFrame;
        }

        internal Row(CastingCommand fixedStatus) : this(fixedStatus, fixedStatus, fixedStatus, fixedStatus) { }
    }
}